using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Asys.System.Environment.Windows.Scheduler.Actions;
using Asys.System.Environment.Windows.Scheduler.Triggers;

namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// The definition of <see cref="TaskDefinition"/> 
/// which represents on how to contruct a <see cref="ITaskScheduler"/> task.
/// Based off of <see cref="Microsoft.Win32.TaskScheduler.TaskDefinition"/>.
/// </summary>
[XmlRoot("Task")]
public sealed class TaskDefinition
{
    /// <summary>
    /// The <see cref="TaskPrincipal"/> for the task that provides the security credentials for the task.
    /// </summary>
    [XmlArray("Principals")]
    [XmlArrayItem]
    public TaskPrincipal Principal { get; } = new TaskPrincipal();

    /// <summary>
    /// The instance of <see cref="TaskRegistrationInformation"/> that is used to describe a task, such as the description of the task, the
    /// author of the task, and the date the task is registered.
    /// </summary>
    public TaskRegistrationInformation RegistrationInformation { get; } = new TaskRegistrationInformation();

    /// <summary>
    /// A collection of <see cref="ITaskTrigger"/ > that are used to start a task.
    /// </summary>
    public ICollection<ITaskTrigger> Triggers { get; } = new Collection<ITaskTrigger>();

    /// <summary>
    /// The collection of <see cref="ITaskAction"/> that are performed by the task.
    /// </summary>
    public ICollection<ITaskAction> Actions { get; } = new Collection<ITaskAction>();
}
