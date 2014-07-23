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
            CancelSchedule = false;
            DebugMode = false;
            DebugInformation = "DebugInformation";
            FreeTime = 10;
            StopSpeed = 5;
        }

        public static Boolean CancelSchedule { get; private set; }
        public static Boolean DebugMode { get; private set; }
        public static String DebugInformation { get; private set; }
        public static Int32 FreeTime { get; private set; }
        public static Int32 StopSpeed { get; private set; }
    }
}
