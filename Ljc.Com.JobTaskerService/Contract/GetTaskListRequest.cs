using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService.Contract
{
    public class GetTaskListRequest
    {
       public string sparkTaskName {
            get;
            set;
        }

        public bool isvalid
        {
            get;
            set;
        }

        public bool running
        {
            get;
            set;
        }
        public int pi
        {
            get;
            set;
        } = 1;

        public int ps
        {
            get;
            set;
        } = 10;
    }
}
