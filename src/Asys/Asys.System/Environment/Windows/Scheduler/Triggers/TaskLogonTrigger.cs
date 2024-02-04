using System.Xml.Serialization;

namespace Asys.System.Environment.Windows.Scheduler.Triggers;

/// <summary>
/// The definition of <see cref="TaskLogonTrigger"/> 
/// which represents a trigger that starts a task when a user logs on.
/// When the Task Scheduler service starts, all logged-on users are
/// enumerated and any tasks registered with logon triggers that match the logged on user are run.
/// Based off of <see cref="Microsoft.Win32.TaskScheduler.LogonTrigger"/>.
/// </summary>
[XmlRoot("LogonTrigger")]
public sealed class TaskLogonTrigger : ITaskTrigger
{
    /// <inheritdoc/>
    public bool Enabled { get; set; }

    /// <summary>
    /// <para>The identifier of the user. For example, "MyDomain\MyName" or for a local account, "Administrator".</para>
    /// <para>This property can be in one of the following formats:</para>
    /// <para>• User name or SID: The task is started when the user logs on to the computer.</para>
    /// <para>• NULL: The task is started when any user logs on to the computer.</para>
    /// </summary>
    /// <remarks>
    /// If you want a task to be triggered when any member of a group logs on to the computer rather than when a specific user logs on,
    /// then do not assign a value to the LogonTrigger.UserId property. Instead, create a logon trigger with an empty
    /// LogonTrigger.UserId property and assign a value to the principal for the task using the Principal.GroupId property.
    /// </remarks>
    public string? UserId { get; set; }
}
