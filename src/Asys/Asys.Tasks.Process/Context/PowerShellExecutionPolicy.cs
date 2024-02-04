namespace Asys.Tasks.Process.Context;

/// <summary>
/// The defintion of <see cref="PowerShellExecutionPolicy"/>.
/// </summary>
/// See https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-5.1
/// for more information about PowerShell's execution policies.
public enum PowerShellExecutionPolicy
{
    /// <summary>
    /// This policy is the default policy
    /// already configured with the system.
    /// It will not override the current policy.
    /// </summary>
    Default,

    /// <summary>
    /// This policy will by pass and will not
    /// log warnings/errors.
    /// </summary>
    ByPass,
}
