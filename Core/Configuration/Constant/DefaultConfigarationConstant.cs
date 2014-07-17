using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration.Constant
{
    class DefaultConfigarationConstant
    {
        static DefaultConfigarationConstant()
        {
            Clid = "19567833";
            ApiKey = "9a90ac5f1419459a99f9804f0255c6b4";
            CancelSchedule = false;
            DebugMode = false;
            DebugInformation = "DebugInformation";
            FreeTime = 10;
            StopSpeed = 5;
        }

        public static String Clid { get; private set; }
        public static String ApiKey { get; private set; }
        public static Boolean CancelSchedule { get; private set; }
        public static Boolean DebugMode { get; private set; }
        public static String DebugInformation { get; private set; }
        public static Int32 FreeTime { get; private set; }
        public static Int32 StopSpeed { get; private set; }
    }
}
