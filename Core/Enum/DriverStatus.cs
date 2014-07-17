using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Enum
{
    public enum DriverStatus
    {
        driving = 1,
        waiting = 2,
        transporting = 3,
        complete = 4,
        cancelled = 5,
        failed = 6,
        free = 7
    }
}
