using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    class AppCenter
    {
        static object locker = new object();
        static bool isrunning = false;
        public static bool RunTask()
        {
            try
            {
                lock (locker)
                {
                    if (isrunning)
                    {
                        return false;
                    }
                    isrunning = true;
                }

                long total = 0;
                var sparklist = TaskJobCore.GetTaskList(null, true, null, 1, int.MaxValue, out total);
                DateTime now = DateTime.Now;
                //TimeSpan lastts = new TimeSpan(TaskSumbitInfo.Instance.LastCheckSubmitTime.Hour, TaskSumbitInfo.Instance.LastCheckSubmitTime.Minute, TaskSumbitInfo.Instance.LastCheckSubmitTime.Second);
                TimeSpan nowts = new TimeSpan(now.Hour, now.Minute, now.Second);

                List<JobTaskEntity> runlist = new List<JobTaskEntity>();

                foreach (var item in sparklist)
                {
                    bool submitflag = item.HandStartFlag;
                    if (!submitflag)
                    {
                        if (item.Strategy == null)
                        {
                            continue;
                        }

                        if (item.Strategy.JustOne && item.State != JobState.None)
                        {
                            continue;
                        }

                        if (item.Strategy.FirstTime > DateTime.Now || item.Strategy.EndTime < DateTime.Now)
                        {
                            continue;
                        }

                        int nowday = (int)DateTime.Now.DayOfWeek;
                        if (item.Strategy.RunDays != null && item.Strategy.RunDays.Length > 0 && !item.Strategy.RunDays.Contains(nowday))
                        {
                            continue;
                        }

                        if (item.State == JobState.Submited || item.State == JobState.Submiting || item.State == JobState.Running)
                        {
                            continue;
                        }

                        if (item.Strategy.Interval > 0)
                        {
                            if (item.LastSuccessTime.AddSeconds(item.Strategy.Interval) <= DateTime.Now)
                            {
                                submitflag = true;
                            }
                        }
                        else if (item.Strategy.RunTime != null && item.Strategy.RunTime.Length > 0)
                        {
                            foreach (var tsstr in item.Strategy.RunTime)
                            {
                                TimeSpan ts;
                                if (TimeSpan.TryParse(tsstr, out ts))
                                {

                                    if (DateTime.Now.Date.Add(ts) > TaskSumbitInfo.Instance.LastCheckSubmitTime && ts <= nowts)
                                    {
                                        submitflag = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (submitflag)
                    {
                        runlist.Add(item);
                    }

                    //if(item.Strategy!=null&&item.Strategy.)
                }

                TaskSumbitInfo.Instance.LastCheckSubmitTime = now;
                TaskSumbitInfo.Instance.Save();

                if (runlist.Count > 0)
                {
                    if (TaskJobCore.BatchSubmiting(runlist.Select(p => p.ID).ToArray()))
                    {
                        ExecuteTask(runlist);
                    }
                }
                return false;
            }
            finally
            {
                isrunning = false;
            }
        }

        private static void ExecuteTask(List<JobTaskEntity> tasks)
        {
            foreach (var item in tasks)
            {
                new Action(() =>
                {
                    bool issuccess = false;
                    try
                    {
                        DateTime start = DateTime.Now;
                        item.LastSumbitTime = start;
                        item.State = JobState.Submited;
                        if (item.HandStartFlag)
                        {
                            item.HandStartFlag = false;
                        }
                        TaskJobCore.UpdateTask(item);

                        if (item.WriteLog)
                        {
                            TaskJobCore.InsertLog(new JobTaskLog
                            {
                                CTime = DateTime.Now,
                                ErrorMsg = "提交成功",
                                FinishTime = DateTime.Now,
                                SubmitTime = DateTime.Now,
                                TaskId = item.ID,
                                IsSuccess = true,
                                TaskName = item.TaskName
                            });
                        }

                        WebClient wc = new WebClient();
                        wc.Encoding = Encoding.UTF8;
                        var html = wc.DownloadString(item.ExecuteUrl);
                        issuccess = true;

                        if (item.WriteLog)
                        {
                            //记录日志
                            TaskJobCore.InsertLog(new JobTaskLog
                            {
                                CTime = DateTime.Now,
                                ErrorMsg = "",
                                Info = html,
                                FinishTime = DateTime.Now,
                                IsSuccess = true,
                                SubmitTime = start,
                                TaskId = item.ID,
                                TaskName = item.TaskName
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskJobCore.InsertLog(new JobTaskLog
                        {
                            CTime = DateTime.Now,
                            ErrorMsg = "任务提交失败:" + ex.ToString(),
                            FinishTime = DateTime.Now,
                            SubmitTime = DateTime.Now,
                            TaskId = item.ID,
                            IsSuccess = false,
                            TaskName = item.TaskName
                        });

                    }
                    finally
                    {
                        item.State = issuccess ? JobState.Success : JobState.Failed;

                        if (issuccess)
                        {
                            item.LastSuccessTime = DateTime.Now;
                            item.TryTimes = 0;
                        }
                        else
                        {
                            item.TryTimes = item.TryTimes + 1;
                        }
                        TaskJobCore.UpdateTask(item.ID, item.State, item.LastSuccessTime, item.TryTimes);
                    }
                }).BeginInvoke(null, null);
            }
        }
    }
}
