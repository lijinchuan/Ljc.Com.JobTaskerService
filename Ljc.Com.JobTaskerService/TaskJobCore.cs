using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    public class TaskJobCore
    {
        static TaskJobCore()
        {
            BigEntityTableEngine.LocalEngine.CreateTable<JobTaskEntity>(p => p.ID, b => 
            b.AddIndex("TaskName", c => c.Asc(d => d.TaskName)));

            BigEntityTableEngine.LocalEngine.CreateTable<JobTaskLog>(p => p.ID, b =>
               b.AddIndex("CTime_-1",c=>c.Asc(d=>d.CTime)));

        }

        public static void ResetHungUpTask()
        {
            long total = 0;
            var tasklist = GetTaskList(null, true, true, 1, int.MaxValue, out total);
            foreach(var task in tasklist)
            {
                task.State = JobState.None;
                BigEntityTableEngine.LocalEngine.Update<JobTaskEntity>(nameof(JobTaskEntity), task);
            }
        }

        private static void Assert(JobTaskEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.TaskName))
            {
                throw new Exception("任务名称不能为空");
            }

            if (entity.Strategy.ThenTaskIds == null)
            {
                entity.Strategy.ThenTaskIds = new string[] { };
            }

            if (entity.Strategy.RunDays == null)
            {
                entity.Strategy.RunDays = new int[] { };
            }

            if (entity.Strategy.RunTime == null)
            {
                entity.Strategy.RunTime = new string[] { };
            }

            if (entity.Strategy.Interval < 0)
            {
                throw new Exception("任务间隔参数不能小于0");
            }

            if (string.IsNullOrWhiteSpace(entity.ExecuteUrl) && string.IsNullOrWhiteSpace(entity.Domain))
            {
                throw new Exception("executeurl或者domain不能为空");
            }
        }

        public static int AddTask(JobTaskEntity entity)
        {
            Assert(entity);

            if (entity.TryTimes < 0)
            {
                entity.TryTimes = 0;
            }


            var one = BigEntityTableEngine.LocalEngine.Find<JobTaskEntity>(nameof(JobTaskEntity), "TaskName",new[] { entity.TaskName}).FirstOrDefault();
            if (one != null)
            {
                throw new Exception("任务已存在:" + entity.TaskName);
            }

            entity.CTime = DateTime.Now;
            entity.LastSumbitTime = DateTime.MinValue;
            BigEntityTableEngine.LocalEngine.Insert<JobTaskEntity>(nameof(JobTaskEntity), entity);

            return entity.ID;
        }

        public static void UpdateTask(int id,JobState taskStatus,DateTime lastSuccessTime,int trytimes)
        {
            var one = BigEntityTableEngine.LocalEngine.Find<JobTaskEntity>(nameof(JobTaskEntity), id);
            if (one == null)
            {
                throw new Exception("任务不存在:" + id);
            }
            if (one.State != taskStatus)
            {
                one.State = taskStatus;
                one.LastSuccessTime = lastSuccessTime;
                one.TryTimes = trytimes;
                BigEntityTableEngine.LocalEngine.Update<JobTaskEntity>(nameof(JobTaskEntity), one);
            }
        }

        public static void UpdateTask(JobTaskEntity entity)
        {
            Assert(entity);

            var one = BigEntityTableEngine.LocalEngine.Find<JobTaskEntity>(nameof(JobTaskEntity),entity.ID);
            if (one == null)
            {
                throw new Exception("任务不存在:" + entity.ID);
            }

            one.Domain= entity.Domain;
            one.IsValid=entity.IsValid;
            one.MainClass=entity.MainClass;
            one.ExecuteUrl=entity.ExecuteUrl;
            one.TaskName=entity.TaskName;
            one.TaskDesc=entity.TaskDesc;
            one.Strategy.JustOne=entity.Strategy.JustOne;
            one.Strategy.ErrorTryTimes=entity.Strategy.ErrorTryTimes;
            one.Strategy.EndTime=entity.Strategy.EndTime;
            one.Strategy.Interval=entity.Strategy.Interval;
            one.Strategy.FirstTime=entity.Strategy.FirstTime;
            one.Strategy.ThenTaskIds=entity.Strategy.ThenTaskIds;
            one.Strategy.RunDays=entity.Strategy.RunDays;
            one.Strategy.RunTime=entity.Strategy.RunTime;
            one.WriteLog=entity.WriteLog;
            one.HandStartFlag = entity.HandStartFlag;
            one.DelProtected=entity.DelProtected;

            BigEntityTableEngine.LocalEngine.Update<JobTaskEntity>(nameof(JobTaskEntity), one);
        }


        public static List<JobTaskEntity> GetTaskList(string sparkTaskName, bool isvalid, bool? running, int pi, int ps, out long total)
        {
            List<JobTaskEntity> allJobTasks;
            total = 0;
            if (!string.IsNullOrWhiteSpace(sparkTaskName))
            {
                allJobTasks = BigEntityTableEngine.LocalEngine.Scan<JobTaskEntity>(nameof(JobTaskEntity), "TaskName", new[] { sparkTaskName },
                    new[] { sparkTaskName }, 1, 1, ref total);
            }
            else
            {
                allJobTasks = BigEntityTableEngine.LocalEngine.List<JobTaskEntity>(nameof(JobTaskEntity), 1, int.MaxValue)
                    .OrderByDescending(p=>p.CTime).ToList();
            }

            allJobTasks = allJobTasks.Where(p => p.IsValid == isvalid).ToList();
            

            if (running.HasValue && running.Value)
            {
                var states= new object[] { JobState.Running, JobState.Submited, JobState.Submiting };
                allJobTasks = allJobTasks.Where(p => states.Contains(p.State)).ToList();
            }

            return allJobTasks;
        }

        public static bool BatchSubmiting(int[] objids)
        {
            var tasks= BigEntityTableEngine.LocalEngine.FindBatch<JobTaskEntity>(nameof(JobTaskEntity), objids.Select(p => (object)p).ToArray()).ToArray();

            foreach(var item in tasks)
            {
                item.State = JobState.Submiting;
                BigEntityTableEngine.LocalEngine.Update<JobTaskEntity>(nameof(JobTaskEntity), item);
            }

            return true;
        }

        public static JobTaskEntity GetTask(int id)
        {
            var one = BigEntityTableEngine.LocalEngine.Find<JobTaskEntity>(nameof(JobTaskEntity), id);

            return one;
        }

        public static void StopTask(int id)
        {
            var one = GetTask(id);
            if (one == null)
            {
                throw new Exception("记录不存在:" + id);
            }

            if (!one.IsValid)
            {
                throw new Exception("已停用:" + id);
            }
            one.IsValid = false;
            BigEntityTableEngine.LocalEngine.Update<JobTaskEntity>(nameof(JobTaskEntity), one);

        }

        public static JobTaskEntity DelSparkTask(int id)
        {
            var one = GetTask(id);
            if (one != null)
            {
                BigEntityTableEngine.LocalEngine.Delete<JobTaskEntity>(nameof(JobTaskEntity), id);
            }
            return one;
        }

        public static bool DelTaskByName(string taskname)
        {
            var one = BigEntityTableEngine.LocalEngine.Find<JobTaskEntity>(nameof(JobTaskEntity), "TaskName", new[] { taskname }).FirstOrDefault();
            if (one != null)
            {
                return BigEntityTableEngine.LocalEngine.Delete<JobTaskEntity>(nameof(JobTaskEntity), one);
            }
            return false;
        }

        public static void InsertLog(JobTaskLog log)
        {
            BigEntityTableEngine.LocalEngine.Insert<JobTaskLog>(nameof(JobTaskLog), log);

        }

        public static void ManualStart(int id)
        {
            var one = GetTask(id);
            if (one == null)
            {
                throw new Exception("任务不存在");
            }
            if (one.HandStartFlag == true)
            {
                throw new Exception("请不要重复操作");
            }

            one.HandStartFlag = true;
            one.IsValid = false;
            BigEntityTableEngine.LocalEngine.Update<JobTaskEntity>(nameof(JobTaskEntity), one);
        }

        public static List<JobTaskLog> GetLog(int id, int pi, int ps, out long total)
        {
            total = 0;
            if (id != 0)
            {
                var one = BigEntityTableEngine.LocalEngine.Find<JobTaskLog>(nameof(JobTaskLog), id);
                if (one != null)
                {
                    total = 1;
                    return new List<JobTaskLog>() { one };
                }
            }

            return BigEntityTableEngine.LocalEngine.ScanDesc<JobTaskLog>(nameof(JobTaskLog), "CTime_-1", new object[] { DateTime.MinValue },
                new object[] { DateTime.MaxValue }, pi, ps, ref total);
        }
    }
}
