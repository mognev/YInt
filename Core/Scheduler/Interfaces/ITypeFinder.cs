using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduler.Interfaces
{
    /// <summary>
    /// Classes implementing this interface provide information about types
    /// to various services in the eWare engine.
    /// </summary>
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType<T>();
    }
}
