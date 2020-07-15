using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    /// <summary>
    /// 任务调试策略
    /// </summary>
    public class JobTaskStrategy
    {
        public int ID
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
        /// 运行间隔
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
        /// 运行时间段
        /// </summary>
        public String[] RunTime
        {
            get;
            set;
        }

        /// <summary>
        /// 执行天
        /// </summary>
        public int[] RunDays
        {
            get;
            set;
        }

        /// <summary>
        /// 前置任务
        /// </summary>
        public string[] PreTaskIds
        {
            get;
            set;
        }

        /// <summary>
        /// 后续启动任务
        /// </summary>
        public string[] ThenTaskIds
        {
            get;
            set;
        }
    }
}
