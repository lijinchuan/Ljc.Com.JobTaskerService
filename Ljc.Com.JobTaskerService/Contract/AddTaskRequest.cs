using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService.Contract
{
    public class AddTaskRequest
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            get;
            set;
        }

        /// <summary>
        /// 任务说明
        /// </summary>
        public string TaskDesc
        {
            get;
            set;
        }

        /// <summary>
        /// 网址调用
        /// </summary>
        public string ExecuteUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 仅调一次
        /// </summary>
        public bool JustOne
        {
            get;
            set;
        }

        /// <summary>
        /// 首次运行时间
        /// </summary>
        public DateTime FirstTime
        {
            get;
            set;
        }

        /// <summary>
        /// 任务执行间隔(秒)
        /// </summary>
        public int Interval
        {
            get;
            set;
        }

        /// <summary>
        /// 第后一次运行时间
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }

        /// <summary>
        /// 错误重试次数
        /// </summary>
        public int ErrorTryTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 运行时间段，如01:30,02:30
        /// </summary>
        public string RunTime
        {
            get;
            set;
        }

        /// <summary>
        /// 执行周天,如1,2,5，表示周一周2周五执行
        /// </summary>
        public string RunDays
        {
            get;
            set;
        }

        public int TryTimes
        {
            get;
            set;
        }

        private bool _isvalid = true;
        public bool IsValid
        {
            get
            {
                return _isvalid;
            }
            set
            {
                _isvalid = value;
            }
        }

        public bool WriteLog
        {
            get;
            set;
        }
    }
}
