using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
#if DEBUG
            TaskJobCore.ResetHungUpTask();
            var timer = LJC.FrameWorkV3.Comm.TaskHelper.SetInterval(1000, () =>
            {
                AppCenter.RunTask();
                return false;
            });
            Console.Read();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
