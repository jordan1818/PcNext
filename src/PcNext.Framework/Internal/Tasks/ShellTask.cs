using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;
using Asys.Tasks.Results;
using Microsoft.Extensions.Logging;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks.Exceptions;

namespace PcNext.Framework.Internal.Tasks;

internal sealed class ShellTask : ITask
{
    private readonly ShellType _shellType;
    private readonly IProcessTaskFactory _processTaskFactory;

    public ShellTask(TaskConfiguration taskConfiguration, ShellType shellType, IProcessTaskFactory processTaskFactory)
    {
        _shellType = shellType;
        _processTaskFactory = processTaskFactory;

        Script = taskConfiguration.GetPropertyValue(nameof(Script));
    }

    internal string? Script { get; }

    public async Task<TaskResults> ExecuteAsync(TaskContext context)
    {
        ProcessTaskResults processTaskResults;
        try
        {
            context.State = TaskState.Preparing;

            if (string.IsNullOrWhiteSpace(Script))
            {
                throw new TaskResultsMissingPropertyException(nameof(Script));
            }

            ProcessTaskContext processTaskContext = _shellType switch
            {
                ShellType.PowerShell => new PowerShellProcessTaskContext(context.Logger) { CancellationToken = context.CancellationToken },
                ShellType.Cmd => new CmdProcessTaskContext(context.Logger) { CancellationToken = context.CancellationToken },
                _ => throw new NotSupportedException($"'{nameof(ShellType)}' is not support of value '{_shellType}'."),
            };

            context.State = TaskState.Running;

            context.Logger.LogDebug("Executing script '{ShellScript}' on '{ShellType}'.", Script, _shellType);

            var processTask = _processTaskFactory.Create();
            processTaskResults = await processTask.ExecuteAsync(processTaskContext);
        }
        catch (TaskResultsException e)
        {
            return TaskResults.Failure(e.Message, e.InnerException);
        }
        catch (Exception e)
        {
            return TaskResults.Failure($"An unhandled exception occurred when executing script '{Script}' on '{_shellType}'.", e);
        }
        finally
        {
            context.State = TaskState.Completed;
        }

        return new TaskResults(processTaskResults.Result, processTaskResults.FailureMessage, processTaskResults.Exception);
    }
}
