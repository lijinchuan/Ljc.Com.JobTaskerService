using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService.Contract
{
    public class GetTaskListResponse
    {
        public List<JobTaskEntity> JobTasks
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
