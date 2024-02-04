using Microsoft.Extensions.Logging;

namespace Asys.Tasks.Process.Context;

/// <summary>
/// A defintion of <see cref="PowerShellProcessTaskContext"/> based off of <see cref="ProcessTaskContext"/>
/// which contains additional Powershell context.
/// </summary>
/// <remarks>
/// See https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_powershell_exe?view=powershell-5.1
/// for more information about the command line arguments.
/// </remarks>
public class PowerShellProcessTaskContext : ProcessTaskContext
{
    /// <summary>
    /// Initializes an instance of <see cref="PowerShellProcessTaskContext"/>.
    /// </summary>
    /// <param name="logger">The logger to be used for the <see cref="PowerShellProcessTask"/> or <see cref="PowerShellProcessTask{TPowerShellProcessTaskContext, TPowerShellProcessTaskResults}"/>.</param>
    public PowerShellProcessTaskContext(ILogger logger)
        : base(logger, "powershell.exe")
    {
    }

    /// <inheritdoc/>
    public override string? FullArugments
    {
        get 
        {
            var powershellArguments = new List<string>();

            if (NoLogo)
            {
                powershellArguments.Add("-NoLogo");
            }

            if (ExecutionPolicy is not PowerShellExecutionPolicy.Default)
            {
                powershellArguments.Add($"-ExecutionPolicy {ExecutionPolicy}");
            }

            powershellArguments.Add("-Command"); // By default, command is set last for the command to come.

            return $"{string.Join(" ", powershellArguments)} {Arguments}";
        }
    }

    /// <summary>
    /// This hides the copyright banner at startup of PowerShell command.
    /// </summary>
    /// <remarks>
    /// See https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_powershell_exe?view=powershell-5.1#-nologo
    /// for more information about this command line argument
    /// </remarks>
    public bool NoLogo { get; set; } = true;

    /// <summary>
    /// This describes the PowerShell command execution policies and explains how to manage them.
    /// By default, it is <see cref="PowerShellExecutionPolicy.ByPass"/>.
    /// </summary>
    /// <remarks>
    /// See https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_powershell_exe?view=powershell-5.1#-executionpolicy-executionpolicy
    /// for more information about this command line argument
    /// </remarks> 
    public PowerShellExecutionPolicy ExecutionPolicy { get; set; } = PowerShellExecutionPolicy.ByPass;
}
