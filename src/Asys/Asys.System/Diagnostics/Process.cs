using Asys.System.Diagnostics.Components;
using SystemProcess = System.Diagnostics.Process;
using SystemProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace Asys.System.Diagnostics;

/// <summary>
/// The implementation of <see cref="IProcess"/> within <see cref="Process"/>.
/// </summary>
public sealed class Process : IProcess
{
    private readonly SystemProcess _process;

    /// <inheritdoc/>
    public event DataReceivedEventHandler? OutputDataReceived;

    /// <inheritdoc/>
    public event DataReceivedEventHandler? ErrorDataReceived;

    /// <summary>
    /// Initializes an instance of <see cref="Process"/>.
    /// </summary>
    /// <param name="information">The <see cref="IProcess"/> information to execute with.</param>
    public Process(ProcessInformation information)
    {
        _process = new SystemProcess
        {
            StartInfo = new SystemProcessStartInfo
            {
                FileName = information.Command,
                Arguments = information.Arguments,
                WorkingDirectory = information.WorkingDirectory,
                UseShellExecute = information.UseShellExecute,
                RedirectStandardOutput = information.RedirectStandardOutput,
                RedirectStandardError = information.RedirectStandardError,
            }
        };

        if (information.RedirectStandardOutput)
        {       
            _process.OutputDataReceived += (s, e) => OutputDataReceived?.Invoke(this, new DataReceivedEventArgs(e.Data));
        }

        if (information.RedirectStandardOutput)
        {
            _process.ErrorDataReceived += (s, e) => ErrorDataReceived?.Invoke(this, new DataReceivedEventArgs(e.Data));
        }
    }

    /// <inheritdoc/>
    public int ExitCode => _process.ExitCode;

    /// <inheritdoc/>
    public bool HasExited => _process.HasExited;

    /// <inheritdoc/>
    public void Dispose() => _process.Dispose();

    /// <inheritdoc/>
    public bool Start() 
    {
        var processStarted = _process.Start();
        if (processStarted)
        {
            if (_process.StartInfo.RedirectStandardOutput)
            {
                _process.BeginOutputReadLine();
            }

            if (_process.StartInfo.RedirectStandardError)
            {
                _process.BeginErrorReadLine();
            }
        }

        return processStarted;
    }

    /// <inheritdoc/>
    public Task WaitForExitAsync(CancellationToken cancellationToken = default) => _process.WaitForExitAsync(cancellationToken);

    /// <inheritdoc/>
    public void Kill(bool entireProcessTree = false) => _process.Kill(entireProcessTree);
}
