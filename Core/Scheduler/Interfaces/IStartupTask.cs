using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduler.Interfaces
{
    public interface IStartupTask
    {
        void Execute();
        int Order { get; }
    }
}
