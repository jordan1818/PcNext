using Asys.Tasks.Contexts;
using Microsoft.Extensions.Logging;

namespace Asys.Tasks.Process.Context;

/// <summary>
/// The definition of <see cref="ProcessTaskContext"/> for <see cref="ProcessTask"/> to consume.
/// Based off of <see cref="TaskContext"/>.
/// </summary>
public class ProcessTaskContext : TaskContext
{
    /// <summary>
    /// Initializes an instance of <see cref="ProcessTaskContext"/>.
    /// </summary>
    /// <param name="logger">The logger to be used for the <see cref="ProcessTask"/>.</param>
    /// <param name="command">The command which the <see cref="ProcessTask"/> will execute.</param>
    public ProcessTaskContext(ILogger logger, string command)
        : base(logger)
    {
        Command = command;
    }

    /// <summary>
    /// The command in which <see cref="ProcessTask"/> will be executing.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// The arguments for the <see cref="Command"/> to be executed with.
    /// </summary>
    public string? Arguments { get; set; }

    /// <summary>
    /// The full arguments for the <see cref="Command"/> to be executed with.
    /// This includes additional settings for other processes. For example PowerShell.
    /// </summary>
    public virtual string? FullArugments => Arguments;

    /// <summary>
    /// The working directory for the <see cref="Command"/> to be executed under.
    /// </summary>
    public string WorkingDirectory { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);

    /// <summary>
    /// The expected exit code for the <see cref="Command"/> to return as. By default is '0'.
    /// </summary>
    public int ExpectedExitCode { get; set; }

    /// <summary>
    /// The timeout for the <see cref="Command"/> to execute within. By default, it is 2 minutes.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// The option to log all output from <see cref="ProcessTask"/> to <see cref="TaskContext.Logger"/>.
    /// By default, it is true.
    /// </summary>
    public bool LogOutput { get; set; } = true;
}
