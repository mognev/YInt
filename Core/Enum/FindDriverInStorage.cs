using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Enum
{
    public enum FindDriverInStorage
    {
        Success = 1,
        NotAllFoundDriver = 2,
        CurrentDriverBusy = 3,
        NotFoundDriver = 4
    }
}
