namespace Asys.System.Environment.Windows.Scheduler
{
    /// <summary>
    /// The definition of <see cref="ITaskScheduler"/> 
    /// which represents <see cref="Microsoft.Win32.TaskScheduler.TaskService"/>.
    /// </summary>
    public interface ITaskScheduler : IDisposable
    {
        /// <summary>
        /// Gets the root folder within the task scheduler.
        /// </summary>
        ITaskFolder RootFolder { get; }
    }
}
