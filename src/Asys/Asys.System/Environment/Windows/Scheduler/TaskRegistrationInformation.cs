using System.Xml.Serialization;

namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// The definition of <see cref="TaskRegistrationInformation"/>
/// which provides the administrative information that can be used to describe the task. This information includes details such as a
/// description of the task, the author of the task, the date the task is registered, and the security descriptor of the task.
/// Based off of <see cref="Microsoft.Win32.TaskScheduler.TaskRegistrationInfo"/>.
/// </summary>
[XmlRoot("RegistrationInfo")]
public sealed class TaskRegistrationInformation
{
    /// <summary>
    /// The description of the task.
    /// </summary>
    public string? Description { get; set; }
}
