using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Scheduler
{
    /// <summary>
    /// Represents task manager.
    /// </summary>
    public class TaskManager
    {
        private static readonly TaskManager _instance = new TaskManager();
        private readonly List<TaskThread> _taskThreads = new List<TaskThread>();

        public static TaskManager Instance
        {
            get { return TaskManager._instance; }
        }

        public void Initialize(System.Xml.XmlNode scheduleTasksNode)
        {
            this._taskThreads.Clear();

            foreach (System.Xml.XmlNode threadNode in scheduleTasksNode.ChildNodes)
            {
                if (threadNode.Name.ToLower() == "thread")
                {
                    TaskThread taskThread = new TaskThread(threadNode);
                    this._taskThreads.Add(taskThread);
                    foreach (System.Xml.XmlNode taskNode in threadNode.ChildNodes)
                    {
                        if (taskNode.Name.ToLower() == "task")
                        {
                            var attribute = taskNode.Attributes["type"];
                            Type taskType = Type.GetType(attribute.Value);
                            if (taskType != null)
                            {
                                Core.Scheduler.Task.Task task = new Core.Scheduler.Task.Task(taskType, taskNode);
                                taskThread.AddTask(task);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Starts task manager (Initializes timer for each task thread).
        /// </summary>
        public void Start()
        {
            foreach (TaskThread taskThread in this._taskThreads)
            {
                taskThread.InitTimer();
            }
        }
        /// <summary>
        /// Stops task manager (Disposes each task thread).
        /// </summary>
        public void Stop()
        {
            foreach (TaskThread taskThread in this._taskThreads)
            {
                taskThread.Dispose();
            }
        }

        internal void ProcessException(Core.Scheduler.Task.Task task, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
