using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    public class JobTaskLog
    {
        public int ID
        {
            get;
            set;
        }

        public int TaskId
        {
            get;
            set;
        }

        public string TaskName
        {
            get;
            set;
        }

        public DateTime SubmitTime
        {
            get;
            set;
        }
        
        public DateTime FinishTime
        {
            get;
            set;
        }

        public bool IsSuccess
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            get;
            set;
        }

        public string Info
        {
            get;
            set;
        }

        public DateTime CTime
        {
            get;
            set;
        }

    }
}
