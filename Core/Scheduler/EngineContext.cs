using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Scheduler.Interfaces;

namespace Core.Scheduler
{
    /// <summary>
    /// Provides access to the singleton instance of the eWare engine.
    /// </summary>
    public class EngineContext
    {
        public static IEngine Initialize()
        {
            if (Singleton<IEngine>.Instance == null)
            {
                YandexServicesConfig config = System.Configuration.ConfigurationManager.GetSection("YandexServices") as YandexServicesConfig;
                Singleton<IEngine>.Instance = new CoreEngine();
                Singleton<IEngine>.Instance.Initialize(config);
            }

            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// Gets the singleton eWare engine used to access the eWare services.
        /// </summary>
        public static IEngine Instance
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                    Initialize();
                return Singleton<IEngine>.Instance;
            }
        }
    }
}
