using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    public partial class Service1 : ServiceBase
    {
        private static int PORT = int.Parse(System.Configuration.ConfigurationManager.AppSettings["port"]);
        private System.Timers.Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        public void Start()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            LJC.FrameWorkV3.HttpApi.APIFactory.Init("Ljc.Com.JobTaskerService");
            TaskJobCore.ResetHungUpTask();
            timer = LJC.FrameWorkV3.Comm.TaskHelper.SetInterval(1000, () =>
               {
                   AppCenter.RunTask();
                   return false;
               });
            AppCenter.RunTask();
        }

        protected override void OnStop()
        {
            BigEntityTableEngine.LocalEngine.ShutDown();
        }
    }
}
