using System.Xml.Serialization;

namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// The definition of <see cref="TaskPrincipal"/> 
/// which represents on how to contruct a <see cref="ITaskScheduler"/> task's principal.
/// Based off of <see cref="Microsoft.Win32.TaskScheduler.TaskPrincipal"/>.
/// </summary>
[XmlRoot("Principal")]
public sealed class TaskPrincipal
{
    /// <summary>
    /// The name of the <see cref="TaskPrincipal"/> that is displayed in the Task Scheduler UI.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The user identifier that is required to run the tasks that are associated with the principal settings this property
    /// to something other than a null or empty string.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The security logon method, <see cref="TaskLogonType"/>, that is required to run the tasks that are associated with the <see cref="TaskPrincipal"/>.
    /// </summary>
    public TaskLogonType LogonType { get; set; }

    /// <summary>
    /// The identifier that is used to specify the privilege level, <see cref="TaskRunLevel"/>, that is required to run the tasks that are associated
    /// with the <see cref="TaskPrincipal"/>.
    /// </summary>
    public TaskRunLevel RunLevel { get; set; }
}
