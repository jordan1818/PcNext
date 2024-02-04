namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// Defines what logon technique is required to run a task.
/// Based off of <see cref="Microsoft.Win32.TaskScheduler.TaskLogonType"/>
/// </summary>
public enum TaskLogonType
{
    /// <summary>
    /// The logon method is not specified. Used for non-NT credentials.
    /// </summary>
    None,

    /// <summary>
    /// Use a password for logging on the user. The password must be supplied at registration time.
    /// </summary>
    Password,

    /// <summary>
    /// Use an existing interactive token to run a task. The user must log on using a service for user (S4U) logon. When an S4U logon is
    /// used, no password is stored by the system and there is no access to either the network or to encrypted files.
    /// </summary>
    S4U,

    /// <summary>
    /// User must already be logged on. The task will be run only in an existing interactive session.
    /// </summary>
    InteractiveToken,

    /// <summary>
    /// Group activation. The groupId field specifies the group.
    /// </summary>
    Group,

    /// <summary>
    /// Indicates that a Local System, Local Service, or Network Service account is being used as a security context to run the task.
    /// </summary>
    ServiceAccount,

    /// <summary>
    /// First use the interactive token. If the user is not logged on (no interactive token is available), then the password is used.
    /// The password must be specified when a task is registered. This flag is not recommended for new tasks because it is less reliable
    /// than Password.
    /// </summary>
    InteractiveTokenOrPassword,
}
