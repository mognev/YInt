namespace Core.Scheduler.Task.Interface
{
    /// <summary>
    /// Interface that should be implemented by each task.
    /// </summary>
    public interface ITask
    {
        void Execute(System.Xml.XmlNode xmlNode);
    }
}
