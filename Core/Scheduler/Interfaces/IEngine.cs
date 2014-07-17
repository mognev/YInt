namespace Core.Scheduler.Interfaces
{
    using Core.Configuration;

    /// <summary>
    /// Classes implementing this interface can serve as a portal for the various
    /// services composing the eWare engine. Edit functionality, modules
    /// and implementations access most eWare functionality through this interface.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Initialize components and plugins in the eWare environment.
        /// </summary>
        /// <param name="config">Config</param>
        void Initialize(YandexServicesConfig config);
    }
}
