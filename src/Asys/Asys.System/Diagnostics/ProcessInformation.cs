using SystemProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace Asys.System.Diagnostics;

/// <summary>
/// The defintion of <see cref="ProcessInformation"/> for <see cref="IProcess"/>
/// to use for execuing.
/// Based off of <see cref="SystemProcessStartInfo"/>.
/// </summary>
public sealed class ProcessInformation
{
    /// <summary>
    /// Initializes a instance of <see cref="ProcessInformation"/>.
    /// </summary>
    /// <param name="command">The command in which the <see cref="IProcess"/> will execute.</param>
    public ProcessInformation(string command)
    {
        Command = command;
    }

    /// <summary>
    /// The command in which the <see cref="IProcess"/> will execute.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// The arguments for <see cref="Comamnd"/> will use to execute with, if any.
    /// </summary>
    public string? Arguments { get; set; }

    /// <summary>
    /// The working directory for the <see cref="Command"/>
    /// to execute under.
    /// </summary>
    public string? WorkingDirectory { get; set; }

    /// <summary>
    /// Indicates whether to use the current system's shell.
    /// </summary>
    public bool UseShellExecute { get; set; }

    /// <summary>
    /// Indicates to redirect the standard error to <see cref="IProcess.ErrorDataReceived"/>.
    /// </summary>
    public bool RedirectStandardError { get; set; }

    /// <summary>
    /// Indicates to redirect the standard error to <see cref="IProcess.OutputDataReceived"/>.
    /// </summary>
    public bool RedirectStandardOutput { get; set; }
}
