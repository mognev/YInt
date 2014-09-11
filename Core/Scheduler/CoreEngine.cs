
namespace Core.Scheduler
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Configuration;
    using Core.Scheduler.Interfaces;

    /// <summary>
    /// eWare engine.
    /// </summary>
    public class CoreEngine : IEngine
    {
        /// <summary>
        /// Initialize components and plugins in the eWare environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize(YandexServicesConfig config)
        {
            StartScheduledTasks(config);
        }

        //TODO: refactor to remove. According to 2.3
        private void StartScheduledTasks(YandexServicesConfig config)
        {
            //initiliaze task manager
            if (config.ScheduleTasks != null)
            {
                TaskManager.Instance.Initialize(config.ScheduleTasks);
                TaskManager.Instance.Start();
            }
        }

       // public Autofac.IContainer Container { get; set; }
    }
}
