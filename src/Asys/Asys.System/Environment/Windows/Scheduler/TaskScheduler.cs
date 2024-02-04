namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// The implementation of <see cref="ITaskScheduler"/> within <see cref="TaskScheduler"/>.
/// </summary>
public sealed class TaskScheduler : ITaskScheduler
{
    private readonly Microsoft.Win32.TaskScheduler.TaskService _taskService;

    /// <summary>
    /// Initializes an instance of <see cref="TaskScheduler"/>.
    /// </summary>
    public TaskScheduler()
    {
        _taskService = new Microsoft.Win32.TaskScheduler.TaskService();
    }

    /// <inheritdoc/>
    public ITaskFolder RootFolder => new TaskFolder(_taskService.RootFolder);

    /// <inheritdoc/>
    public void Dispose() => _taskService.Dispose();
}
