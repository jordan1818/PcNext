using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Asys.System;
using ChocolateySharp;
using ChocolateySharp.Options;
using Microsoft.Extensions.Logging;
using PcNext.Framework.Configurations;

namespace PcNext.Framework.Internal.Tasks;

internal sealed class ChocolateyTask : ITask
{
    private readonly IChocolateyClient _chocolateyClient;
    private readonly ITaskConfigurationTaskFactory _taskConfigurationTaskFactory;
    private readonly ChocolateyConfiguration _chocolateyConfiguration;

    public ChocolateyTask(ChocolateyConfiguration chocolateyConfiguration, IChocolateyClient chocolateyClient, ITaskConfigurationTaskFactory taskConfigurationTaskFactory)
    {
        _chocolateyClient = chocolateyClient;
        _taskConfigurationTaskFactory = taskConfigurationTaskFactory;
        _chocolateyConfiguration = chocolateyConfiguration;
    }

    public async Task<TaskResults> ExecuteAsync(TaskContext context)
    {
        try
        {
            context.State = TaskState.Preparing;

            if (string.IsNullOrWhiteSpace(_chocolateyConfiguration.Name))
            {
                throw new TaskResultsException("Chocolatey package name must be set.");
            }

            context.State = TaskState.Running;

            if (_chocolateyConfiguration.BeforeTasks.Any())
            {
                context.Logger.LogDebug("Executing {BeforeTaskCount} before tasks for Chocolatey package '{Name}'.", _chocolateyConfiguration.BeforeTasks.Count(), _chocolateyConfiguration.Name);
                foreach (var beforeTaskConfiguration in _chocolateyConfiguration.BeforeTasks)
                {
                    context.Logger.LogDebug("Executing before task '{BeforeTaskName}' for Chocolatey package '{Name}'.", beforeTaskConfiguration.Name, _chocolateyConfiguration.Name);

                    var beforeTask = _taskConfigurationTaskFactory.Create(beforeTaskConfiguration);

                    var beforeTaskContext = new TaskContext(context.Logger)
                    {
                        CancellationToken = context.CancellationToken
                    };

                    var beforeTaskResults = await beforeTask.ExecuteAsync(beforeTaskContext);
                    if (!beforeTaskResults.IsSuccessful)
                    {
                        throw new TaskResultsException($"Before task '{beforeTaskConfiguration.Name}' has failed with message '{beforeTaskResults.FailureMessage}'.", beforeTaskResults.Exception);
                    }
                }
            }

            context.Logger.LogDebug("Installing Chocolatey package '{Name}'", _chocolateyConfiguration.Name);

            var isInstalled = await _chocolateyClient.InstallPackageAsync(_chocolateyConfiguration.Name, new ChocolateyPackageInstallOptions
            {
                Version = SemanticVersion.TryParse(_chocolateyConfiguration.Version, out var v) ? v : null,
                PackageParameters = _chocolateyConfiguration.PackageParameters,
            }, context.CancellationToken);

            if (!isInstalled)
            {
                throw new TaskResultsException($"Chocolatey package '{_chocolateyConfiguration.Name}' was not successfully installed.");
            }

            if (_chocolateyConfiguration.AfterTasks.Any())
            {
                context.Logger.LogDebug("Executing {AfterTaskCount} after tasks for Chocolatey package '{Name}'.", _chocolateyConfiguration.AfterTasks.Count(), _chocolateyConfiguration.Name);
                foreach (var afterTaskConfiguration in _chocolateyConfiguration.AfterTasks)
                {
                    context.Logger.LogDebug("Executing after task '{AfterTaskName}' for Chocolatey package '{Name}'.", afterTaskConfiguration.Name, _chocolateyConfiguration.Name);

                    var afterTask = _taskConfigurationTaskFactory.Create(afterTaskConfiguration);

                    var afterTaskContext = new TaskContext(context.Logger)
                    {
                        CancellationToken = context.CancellationToken
                    };

                    var afterTaskResults = await afterTask.ExecuteAsync(afterTaskContext);
                    if (!afterTaskResults.IsSuccessful)
                    {
                        throw new TaskResultsException($"After task '{afterTaskConfiguration.Name}' has failed with message '{afterTaskResults.FailureMessage}'.", afterTaskResults.Exception);
                    }
                }
            }
        }
        catch (TaskResultsException e)
        {
            return TaskResults.Failure(e.Message, e.InnerException);
        }
        catch (Exception e)
        {
            return TaskResults.Failure($"A unhandled exception occurred when installing Chocolatey package '{_chocolateyConfiguration.Name}'.", e);
        }
        finally
        {
            context.State = TaskState.Completed;
        }

        return TaskResults.Success();
    }
}
