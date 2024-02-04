using Asys.System.Diagnostics.Components;

using SystemProcess = System.Diagnostics.Process;

namespace Asys.System.Diagnostics;

/// <summary>
/// The definition of <see cref="IProcess"/>
/// which represents the system's process operations.
/// Based off of <see cref="SystemProcess"/>.
/// </summary>
public interface IProcess : IDisposable
{
    /// <summary>
    /// The event handler for standard output of the <see cref="IProcess"/>. If <see cref="ProcessInformation.RedirectStandardOutput"/> is enabled.
    /// </summary>
    event DataReceivedEventHandler? OutputDataReceived;

    /// <summary>
    /// The event handler for standard error of the <see cref="IProcess"/>. If <see cref="ProcessInformation.RedirectStandardError"/> is enabled.
    /// </summary>
    event DataReceivedEventHandler? ErrorDataReceived;

    /// <summary>
    /// The exit code of the <see cref="IProcess"/> execution.
    /// </summary>
    int ExitCode { get; }

    /// <summary>
    /// Indicates whether the <see cref="IProcess"/> has exited.
    /// </summary>
    bool HasExited { get; }

    /// <summary>
    /// Starts the <see cref="IProcess"/>.
    /// </summary>
    /// <returns>Returns true if the <see cref="IProcess"/> has started successfully; otherwise false.</returns>
    bool Start();

    /// <summary>
    /// Asynchronously wait for <see cref="IProcess"/> to finish executing.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the <see cref="IProcess"/>.</param>
    /// <returns></returns>
    Task WaitForExitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Kills the <see cref="IProcess"/> execution.
    /// </summary>
    /// <param name="entireProcessTree">Indicates whether to kill the entire process tree.</param>
    void Kill(bool entireProcessTree = false);
}
