using SystemEnvironmentVariableTarget = System.EnvironmentVariableTarget;

namespace Asys.System.Environment;

/// <summary>
/// Specifies the location where an environment variable is stored or retrieved in
/// a set or get operation.
/// Base off of <see cref="SystemEnvironmentVariableTarget"/>.
/// </summary>
public enum EnvironmentVariableTarget
{
    // Summary:
    //     The environment variable is stored or retrieved from the environment block associated
    //     with the current process.
    /// <summary>
    /// The environment variable is stored or retrieved from the environment block associated
    /// with the current process.
    /// </summary>
    Process,

    // Summary:
    //     The environment variable is stored or retrieved from the HKEY_CURRENT_USER\Environment
    //     key in the Windows operating system registry. This value should be used on .NET
    //     implementations running on Windows systems only.
    /// <summary>
    /// The environment variable is stored or retrieved from the HKEY_CURRENT_USER\Environment
    /// key in the Windows operating system registry. This value should be used on .NET
    /// implementations running on Windows systems only.
    /// </summary>
    User,

    /// <summary>
    /// The environment variable is stored or retrieved from the HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session
    /// Manager\Environment key in the Windows operating system registry. This value
    /// should be used on .NET implementations running on Windows systems only.
    /// </summary>
    Machine,
}
