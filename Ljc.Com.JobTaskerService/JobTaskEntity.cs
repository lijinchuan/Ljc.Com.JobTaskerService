using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    public class JobTaskEntity 
    {

        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            get;
            set;
        }

        public string TaskDesc
        {
            get;
            set;
        }

        public string ExecuteUrl
        {
            get;
            set;
        }

        public string Domain
        {
            get;
            set;
        }

        public String MainClass
        {
            get;
            set;
        }

        public JobTaskStrategy Strategy
        {
            get;
            set;
        }

        private JobState _state = JobState.None;
        public JobState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public DateTime CTime
        {
            get;
            set;
        }

        public DateTime LastSumbitTime
        {
            get;
            set;
        }

        public DateTime LastSuccessTime
        {
            get;
            set;
        }

        public int TryTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 手工启动标记
        /// </summary>
        public bool HandStartFlag
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


        private bool _delProtected = false;
        public bool DelProtected
        {
            get
            {
                return _delProtected;
            }
            set
            {
                _delProtected = value;
            }
        }
    }
}
