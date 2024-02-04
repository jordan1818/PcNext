using Asys.System.Extensions;

namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// The implementation of <see cref="ITaskFolder"/> within <see cref="TaskFolder"/>.
/// </summary>
public sealed class TaskFolder : ITaskFolder
{
    private readonly Microsoft.Win32.TaskScheduler.TaskFolder _taskFolder;

    /// <summary>
    /// Initializes an instance of <see cref="TaskFolder"/>.
    /// </summary>
    /// <param name="taskFolder">The instance of <see cref="Microsoft.Win32.TaskScheduler.TaskFolder"/>.</param>
    public TaskFolder(Microsoft.Win32.TaskScheduler.TaskFolder taskFolder)
    {
        _taskFolder = taskFolder;
    }

    /// <inheritdoc/>
    public void Dispose() => _taskFolder.Dispose();

    /// <inheritdoc/>
    public void Register(string path, TaskDefinition taskDefinition) => _taskFolder.RegisterTask(path, taskDefinition.ToXmlString());
}
