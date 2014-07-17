using System;
using System.Collections.Generic;

namespace Core.Scheduler
{
    /// <summary>
    /// Represents task thread.
    /// </summary>
    public class TaskThread : IDisposable
    {
        private System.Threading.Timer _timer;
        private bool _disposed;
        private readonly Dictionary<string, Core.Scheduler.Task.Task> _tasks;
        private readonly int _seconds;

        public TaskThread()
        {
            this._tasks = new Dictionary<string, Core.Scheduler.Task.Task>();
            this._seconds = (int)(0.5 * 60);
        }

        internal TaskThread(System.Xml.XmlNode threadNode)
        {
            this._tasks = new Dictionary<string, Core.Scheduler.Task.Task>();
            this._seconds = (int)(0.5 * 60);
            if (threadNode.Attributes["seconds"] != null &&
                !int.TryParse(threadNode.Attributes["seconds"].Value, out this._seconds))
            {
                this._seconds = 60 * 10;
            }
        }

        public void Dispose()
        {
            if (this._timer != null && !this._disposed)
            {
                lock (this)
                {
                    this._timer.Dispose();
                    this._timer = null;
                    this._disposed = true;
                }
            }
        }

        internal void AddTask(Core.Scheduler.Task.Task task)
        {
            if (!this._tasks.ContainsKey(task.Name))
            {
                this._tasks.Add(task.Name, task);
            }
        }

        internal void InitTimer()
        {
            if (this._timer == null)
            {
                this._timer = new System.Threading.Timer(
                    new System.Threading.TimerCallback(this.TimerHandler), null, this.Interval, this.Interval);
            }
        }

        private void TimerHandler(object state)
        {
            this._timer.Change(-1, -1);
            this.Run();
            this._timer.Change(this.Interval, this.Interval);
        }

        private void Run()
        {
            foreach (Core.Scheduler.Task.Task task in this._tasks.Values)
            {
                task.Execute();
            }
        }

        public long Interval
        {
            get
            {
                return this._seconds * 1000;
            }
        }
    }
}
