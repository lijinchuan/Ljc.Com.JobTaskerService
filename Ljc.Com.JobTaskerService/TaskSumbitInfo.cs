using LJC.FrameWorkV3.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ljc.Com.JobTaskerService
{
    [Serializable]
    public class TaskSumbitInfo
    {
        private static readonly String FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\TaskSumbitInfo.xml";
        public static TaskSumbitInfo Instance = null;

        static TaskSumbitInfo()
        {
            Instance = SerializerHelper.DeSerializerFile<TaskSumbitInfo>(FilePath, true)
                ?? new TaskSumbitInfo() { LastCheckSubmitTime = DateTime.Now };
        }

        public DateTime LastCheckSubmitTime
        {
            get;
            set;
        }

        public void Save()
        {
            SerializerHelper.SerializerToXML(this, FilePath);
        }

    }
}
