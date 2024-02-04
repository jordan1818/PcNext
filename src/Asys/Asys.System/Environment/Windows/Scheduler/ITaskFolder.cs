namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// The definition of <see cref="ITaskFolder"/>
/// which represents <see cref="Microsoft.Win32.TaskScheduler.TaskFolder"/>.
/// </summary>
public interface ITaskFolder : IDisposable
{
    /// <summary>
    /// Registers (creates) a task in a specified location using a <see cref="TaskDefinition"/> instance to define a task.
    /// </summary>
    /// <param name="path">
    /// The task name. If this value is NULL, the task will be registered in the root task folder and the task name will be a GUID value that is created by the Task Scheduler service. 
    /// A task name cannot begin or end with a space character. 
    /// The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.
    /// </param>
    /// <param name="definition">The <see cref="TaskDefinition"/> of the registered task.</param>
    void Register(string path, TaskDefinition taskDefinition);
}
