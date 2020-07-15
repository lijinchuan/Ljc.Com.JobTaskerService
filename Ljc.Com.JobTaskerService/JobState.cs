using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    public enum JobState
    {
        None,
        Submiting,
        Submited,
        Running,
        Failed,
        Success
    }
}
