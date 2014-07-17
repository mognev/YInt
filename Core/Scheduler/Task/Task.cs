
namespace Core.Scheduler.Task
{
    using System;
    using Core.Scheduler.Task.Interface;
    
    /// <summary>
    /// Task
    /// </summary>
    public class Task
    {
        private bool _enabled;
        private readonly bool _stopOnError;
        private readonly string _name;

        public string Name
        {
            get { return _name; }
        }

        private readonly Type _taskType;
        private readonly System.Xml.XmlNode _taskNode;
        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }
        private DateTime _lastStarted;
        private DateTime _lastSuccess;
        private DateTime _lastEnd;
        private ITask _task;

        public Task(Type taskType, System.Xml.XmlNode taskNode)
        {
            this._enabled = true;
            this._taskType = taskType;
            this._taskNode = taskNode;
            if (taskNode.Attributes["enabled"] != null &&
                !bool.TryParse(taskNode.Attributes["enabled"].Value, out this._enabled))
            {
                this._enabled = true;
            }
            if (taskNode.Attributes["stopOnError"] != null &&
                !bool.TryParse(taskNode.Attributes["stopOnError"].Value, out this._stopOnError))
            {
                this._stopOnError = true;
            }
            if (taskNode.Attributes["name"] != null)
            {
                this._name = taskNode.Attributes["name"].Value;
            }
        }



        internal void Execute()
        {
            this._isRunning = true;
            try
            {
                ITask task = this.createTask();
                if (task != null)
                {
                    this._lastStarted = DateTime.Now;
                    task.Execute(this._taskNode);
                    this._lastEnd = this._lastSuccess = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                this._enabled = !this._stopOnError;
                this._lastEnd = DateTime.Now;
                TaskManager.Instance.ProcessException(this, ex);
            }
            this._isRunning = false;
        }

        private ITask createTask()
        {
            if (this._enabled && (this._task == null))
            {
                if (this._taskType != null)
                {
                    this._task = Activator.CreateInstance(this._taskType) as ITask;
                }
                this._enabled = this._task != null;
            }
            return this._task;
        }


    }
}
