using Ljc.Com.JobTaskerService.Contract;
using LJC.FrameWorkV3.HttpApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    public class ApiDomain
    {
        [APIMethod]
        public int AddTask(AddTaskRequest request)
        {
            int trytimes = 0;
            if (request.TryTimes < 0)
            {
                trytimes = 0;
            }

            JobTaskEntity entity = new JobTaskEntity()
            {
                ExecuteUrl = request.ExecuteUrl,
                State = JobState.None,
                TaskName = request.TaskName,
                TryTimes = trytimes,
                TaskDesc = request.TaskDesc,
                CTime = DateTime.Now,
                LastSuccessTime = DateTime.Now,
                LastSumbitTime = DateTime.Now,
                Strategy = new JobTaskStrategy
                {
                    EndTime = request.EndTime,
                    FirstTime = request.FirstTime,
                    Interval = request.Interval,
                    JustOne = request.JustOne,
                    ErrorTryTimes = trytimes,
                    RunDays = request.RunDays.Trim().Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray(),
                    RunTime = request.RunTime.Trim().Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)
                },
                WriteLog = request.WriteLog
            };

            int id = TaskJobCore.AddTask(entity);

            return id;
        }

        [APIMethod]
        public bool UpdateTask(UpdateTaskRequest request)
        {
            JobTaskEntity entity = new JobTaskEntity()
            {
                ID=request.TaskId,
                MainClass = request.MainClass,
                Domain = request.Domain,
                ExecuteUrl = request.ExecuteUrl,
                State = JobState.None,
                TaskName = request.TaskName,
                TryTimes = request.TryTimes,
                TaskDesc = request.TaskDesc,
                CTime = DateTime.Now,
                LastSuccessTime = DateTime.Now,
                LastSumbitTime = DateTime.Now,
                Strategy = new JobTaskStrategy
                {
                    EndTime = request.EndTime,
                    FirstTime = request.FirstTime,
                    Interval = request.Interval,
                    JustOne = request.JustOne,
                    ErrorTryTimes = request.ErrorTryTimes,
                    RunDays = request.RunDays.Trim().Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray(),
                    RunTime = request.RunTime.Trim().Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)
                },
                WriteLog = request.WriteLog
            };

            TaskJobCore.UpdateTask(entity);

            return true;
        }

        [APIMethod]
        public GetTaskListResponse GetTaskList(GetTaskListRequest request)
        {
            long total = 0;
            var list = TaskJobCore.GetTaskList(request.sparkTaskName, request.isvalid,
                request.running, request.pi, request.ps, out total);

            return new GetTaskListResponse
            {
                Total=total,
                JobTasks=list
            };
        }

        [APIMethod]
        public bool StopTask(StopTaskRequest request)
        {
            TaskJobCore.StopTask(request.TaskId);
            return true;
        }

        [APIMethod]
        public bool DelTask(DelTaskRequest request)
        {
            TaskJobCore.DelSparkTask(request.TaskId);
            return true;
        }

        [APIMethod]
        public bool HandStart(HandStartRequest request)
        {
            TaskJobCore.ManualStart(request.TaskId);

            return true;
        }

        

        [APIMethod]
        public GetLogResponse GetLog(GetLogRequest request)
        {
            long total = 0;
            var list = TaskJobCore.GetLog(request.id, request.pi, request.ps, out total);

            return new GetLogResponse
            {
                JobTaskLogs=list,
                Total=total
            };
        }
    }
}
