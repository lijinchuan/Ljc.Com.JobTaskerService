using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService.Contract
{
    public class GetLogResponse
    {
        public List<JobTaskLog> JobTaskLogs
        {
            get;
            set;
        }

        public long Total
        {
            get;
            set;
        }
    }
}
