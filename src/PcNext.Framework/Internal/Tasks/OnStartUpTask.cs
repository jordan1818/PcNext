using Asys.System.Environment;
using Asys.System.Environment.Windows.Scheduler;
using Asys.System.Environment.Windows.Scheduler.Actions;
using Asys.System.Environment.Windows.Scheduler.Triggers;
using Asys.System.Security;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks.Exceptions;

namespace PcNext.Framework.Internal.Tasks;

internal sealed class OnStartUpTask : ITask
{
    private readonly TaskConfiguration _taskConfiguration;
    private readonly ITaskScheduler _taskScheduler;
    private readonly IOperatingSystem _operatingSystem;
    private readonly IAccountManager _accountManager;

    public OnStartUpTask(TaskConfiguration taskConfiguration, ITaskScheduler taskScheduler, IOperatingSystem operatingSystem, IAccountManager accountManager)
    {
        _taskConfiguration = taskConfiguration;
        _taskScheduler = taskScheduler;
        _operatingSystem = operatingSystem;
        _accountManager = accountManager;

        Folder = taskConfiguration.GetPropertyValue(nameof(Folder));
        Command = taskConfiguration.GetPropertyValue(nameof(Command));
        Arguments = taskConfiguration.GetPropertyValue(nameof(Arguments));
    }

    public string? Folder { get; }

    public string? Command { get; }

    public string? Arguments { get; }

    public Task<TaskResults> ExecuteAsync(TaskContext context)
    {
        return Task.Run(() =>
        {
            try
            {
                context.State = TaskState.Preparing;

                var accountIdentity = _accountManager.GetCurrentIdentity();

                if (!_operatingSystem.IsWindows())
                {
                    throw new TaskResultsException($"The current operating is not running on Windows. Cannot run '{_taskConfiguration.Name}' task.");
                }

                if (accountIdentity is null)
                {
                    throw new TaskResultsException($"The current account identity could not be determined. Cannot run '{_taskConfiguration.Name}' task.");
                }

                if (string.IsNullOrWhiteSpace(Folder))
                {
                    throw new TaskResultsMissingPropertyException(nameof(Folder));
                }

                if (string.IsNullOrWhiteSpace(Command))
                {
                    throw new TaskResultsMissingPropertyException(nameof(Command));
                }

                var taskDefinition = new TaskDefinition();

                taskDefinition.RegistrationInformation.Description = _taskConfiguration.Description;
                taskDefinition.Principal.DisplayName = _taskConfiguration.Name;

                taskDefinition.Principal.UserId = accountIdentity.Id;
                taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
                taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;

                taskDefinition.Triggers.Add(new TaskLogonTrigger
                {
                    Enabled = true,
                    UserId = accountIdentity.FullName,
                });

                taskDefinition.Actions.Add(new TaskExecuteAction(Command, Arguments));

                context.State = TaskState.Running;

                _taskScheduler.RootFolder.Register(Folder, taskDefinition);
            }
            catch (TaskResultsException e)
            {
                return TaskResults.Failure(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                return TaskResults.Failure($"An unhandled exception occurred create/updating schedule task '{_taskConfiguration.Name}' on start up.", e);
            }
            finally
            {
                context.State = TaskState.Completed;
            }

            return TaskResults.Success();
        });
    }
}
