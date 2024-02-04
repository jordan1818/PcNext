using Asys.System.Diagnostics;
using Asys.Tasks.Contexts;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Asys.Tests")]

namespace Asys.Tasks.Process;

/// <summary>
/// The definition of <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/> based off of <see cref="ITask{TTaskContext, TTaskContext}"/>
/// for process related task executions.
/// </summary>
/// <typeparam name="TProcessTaskContext">The process task context which must be derived from <see cref="ProcessTaskContext"/>.</typeparam>
/// <typeparam name="TProcessTaskResults">The process task results which must be derived from <see cref="ProcessTaskResults"/>.</typeparam>
public class ProcessTask<TProcessTaskContext, TProcessTaskResults> : ITask<TProcessTaskContext, TProcessTaskResults>
    where TProcessTaskContext : ProcessTaskContext
    where TProcessTaskResults : ProcessTaskResults
{
    private readonly IProcessFactory _processFactory;

    /// <summary>
    /// Initializes an instance of <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/>.
    /// </summary>
    public ProcessTask()
        : this(processFactory: null)
    {

    }

    /// <summary>
    /// Initializes an instance of <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/>.
    /// </summary>
    /// <param name="processFactory">The instance of <see cref="IProcessFactory"/> for process operations.</param>
    internal ProcessTask(IProcessFactory? processFactory)
    {
        _processFactory = processFactory ?? new ProcessFactory();
    }

    /// <inheritdoc/>
    public virtual async Task<TProcessTaskResults> ExecuteAsync(TProcessTaskContext context)
    {
        context.State = TaskState.Preparing;

        context.Logger.LogDebug("Prepaing '{Command} {Arguments}' in working directory '{WorkingDirectoy}'", context.Command, context.FullArugments, context.WorkingDirectory);

        var standardOutput = string.Empty;
        var standardError= string.Empty;
        using var process = _processFactory.Create(new ProcessInformation(context.Command)
        {
            Arguments = context.FullArugments,
            WorkingDirectory = context.WorkingDirectory,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        });

        process.OutputDataReceived += (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                standardOutput += e.Data;

                if (context.LogOutput)
                {
                    context.Logger.LogInformation("[{Command}] {Message}", context.Command, e.Data);
                }
            }
        };

        process.ErrorDataReceived += (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                standardError += e.Data;

                if (context.LogOutput)
                {
                    context.Logger.LogError("[{Command}] {Message}", context.Command, e.Data);
                }
            }
        };

        context.Logger.LogDebug("Executing '{Command} {Arguments}' in working directory '{WorkingDirectory}'", context.Command, context.FullArugments, context.WorkingDirectory);

        var stopwatch = new Stopwatch();

        using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(context.CancellationToken))
        {
            try
            {
                if (!process.Start())
                {
                    return (TProcessTaskResults)ProcessTaskResults.Failure($"Failed to start '{context.Command} {context.FullArugments}' in working directory '{context.WorkingDirectory}'.");
                }

                stopwatch.Start();

                context.State = TaskState.Running;

                cancellationTokenSource.CancelAfter(context.Timeout);
                await process.WaitForExitAsync(cancellationTokenSource.Token);
            }
            catch (TaskCanceledException e) when (e.InnerException is TimeoutException)
            {
                return (TProcessTaskResults)ProcessTaskResults.Failure($"Timeout has occured after {context.Timeout.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} miliseconds when executing '{context.Command} {context.FullArugments}' in working directory '{context.WorkingDirectory}'.", e.InnerException);
            }
            catch (TaskCanceledException e)
            {
                return (TProcessTaskResults)ProcessTaskResults.Failure($"Cancellation has been requested for '{context.Command} {context.FullArugments}' in working directory '{context.WorkingDirectory}'.", e);
            }
            catch (Exception e)
            {
                return (TProcessTaskResults)ProcessTaskResults.Failure($"'{context.Command} {context.FullArugments}' in working directory '{context.WorkingDirectory}' had an unexpected exception.", e);
            }
            finally
            {
                context.State = TaskState.Completed;

                stopwatch.Stop();

                context.Logger.LogDebug("Excution of '{Command} {Arguments}' in working directory '{WorkingDirectory}' took {TotalMiliSeconds} milliseconds.", context.Command, context.FullArugments, context.WorkingDirectory, stopwatch.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture));

                if (!process.HasExited)
                {
                   context.Logger.LogDebug("Excution of '{Command} {Arguments}' in working directory '{WorkingDirectory}' is currently still running after execution. Stopping the command.", context.Command, context.FullArugments, context.WorkingDirectory);
                   process.Kill(entireProcessTree: true);
                }
            }

            if (process.ExitCode != context.ExpectedExitCode)
            {
                return (TProcessTaskResults)ProcessTaskResults.Failure($"'{context.Command} {context.FullArugments}' in working directory '{context.WorkingDirectory}' exited with '{process.ExitCode}' but expected '{context.ExpectedExitCode}'.");
            }
        }

        return (TProcessTaskResults)ProcessTaskResults.Success(standardOutput, standardError);
    }
}