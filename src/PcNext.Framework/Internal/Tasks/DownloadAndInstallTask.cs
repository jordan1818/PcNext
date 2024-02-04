using Asys.System.IO;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Results;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks.Exceptions;

namespace PcNext.Framework.Internal.Tasks;

internal sealed class DownloadAndInstallTask : ITask
{
    private readonly TaskConfiguration _taskConfiguration;
    private readonly IFileSystem _fileSystem;
    private readonly ITaskConfigurationTaskFactory _taskConfigurationTaskFactory;
    private readonly IProcessTaskFactory _processTaskFactory;

    public DownloadAndInstallTask(TaskConfiguration taskConfiguration, IFileSystem fileSystem, ITaskConfigurationTaskFactory taskConfigurationTaskFactory, IProcessTaskFactory processTaskFactory)
    {
        _fileSystem = fileSystem;
        _taskConfigurationTaskFactory = taskConfigurationTaskFactory;
        _processTaskFactory = processTaskFactory;
        _taskConfiguration = taskConfiguration;

        InstallerUri = taskConfiguration.GetPropertyUriValue(nameof(InstallerUri));
        InstallerArguments = taskConfiguration.GetPropertyValue(nameof(InstallerArguments));
    }

    internal Uri? InstallerUri { get; }

    internal string? InstallerArguments { get; }

    public async Task<TaskResults> ExecuteAsync(TaskContext context)
    {
        try
        {
            context.State = TaskState.Preparing;

            if (InstallerUri is null)
            {
                throw new TaskResultsMissingOrInvalidPropertyException(nameof(InstallerUri), InstallerUri);
            }

            if (string.IsNullOrEmpty(Path.GetExtension(InstallerUri.AbsolutePath)))
            {
                throw new TaskResultsPropertyExpectationException(nameof(InstallerUri), InstallerUri, "http://uri.com/installer.exe");
            }

            using var temporaryDirectory = _fileSystem.CreateTemporaryDirectory();

            var installerFileName = Path.GetFileName(InstallerUri.AbsolutePath);
            var installerFilePath = Path.Combine(temporaryDirectory.Path, installerFileName);
            var installCommand = $"{installerFilePath}{$" {(!string.IsNullOrWhiteSpace(InstallerArguments) ? InstallerArguments : string.Empty)}"}";

            context.State = TaskState.Running;

            var downloadTaskConfiguration = new TaskConfiguration
            {
                Type = TaskType.Download, // Use to retrieve 'download' task.
                Name = _taskConfiguration.Name,
                Description = _taskConfiguration.Description,
                Properties = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    [nameof(DownloadTask.Uri)] = InstallerUri.ToString(),
                    [nameof(DownloadTask.Destination)] = temporaryDirectory.Path,
                },
            };

            var downloadTaskContext = new TaskContext(context.Logger);
            var downloadTask = _taskConfigurationTaskFactory.Create(downloadTaskConfiguration);

            var downloadTaskResults = await downloadTask.ExecuteAsync(downloadTaskContext);
            if (!downloadTaskResults.IsSuccessful)
            {
                throw new TaskResultsException(downloadTaskResults.FailureMessage, downloadTaskResults.Exception);
            }

            var cmdProcessTaskContext = new CmdProcessTaskContext(context.Logger)
            {
                Arguments = installCommand,
            };

            var processTask = _processTaskFactory.Create();
            var processTaskResults = await processTask.ExecuteAsync(cmdProcessTaskContext);
            if (!processTaskResults.IsSuccessful)
            {
                throw new TaskResultsException(processTaskResults.FailureMessage, processTaskResults.Exception);
            }
        }
        catch (TaskResultsException e)
        {
            return TaskResults.Failure(e.Message, e.InnerException);
        }
        catch (Exception e)
        {
            return TaskResults.Failure($"A unhandled exception occurred when downloading and installing '{InstallerUri}'.", e);
        }
        finally
        {
            context.State = TaskState.Completed;
        }

        return TaskResults.Success();
    }
}
