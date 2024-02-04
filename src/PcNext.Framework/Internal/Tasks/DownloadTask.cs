using Asys.System.IO;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Microsoft.Extensions.Logging;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks.Exceptions;

namespace PcNext.Framework.Internal.Tasks;

internal sealed class DownloadTask : ITask
{
    private readonly HttpClient _httpClient;
    private readonly IFileSystem _fileSystem;

    public DownloadTask(TaskConfiguration taskConfiguration, HttpClient httpClient, IFileSystem fileSystem)
    {
        _httpClient = httpClient;
        _fileSystem = fileSystem;

        Uri = taskConfiguration.GetPropertyUriValue(nameof(Uri));
        Destination = taskConfiguration.GetPropertyValue(nameof(Destination));
        OverwriteFile = taskConfiguration.GetPropertyBoolValue(nameof(OverwriteFile));
    }

    internal Uri? Uri { get; }

    internal string? Destination { get; }

    internal bool OverwriteFile { get; }

    public async Task<TaskResults> ExecuteAsync(TaskContext context)
    {
        try
        {
            context.State = TaskState.Preparing;

            if (Uri is null)
            {
                throw new TaskResultsMissingOrInvalidPropertyException(nameof(Uri), Uri);
            }
            
            if (string.IsNullOrEmpty(Path.GetExtension(Uri.AbsolutePath)))
            {
                throw new TaskResultsPropertyExpectationException(nameof(Uri), Uri, "http://uri.com/file.txt");
            }

            if (string.IsNullOrWhiteSpace(Destination))
            {
                throw new TaskResultsMissingPropertyException(nameof(Destination));
            }

            if (!Path.IsPathRooted(Destination))
            {
                throw new TaskResultsException($"'{nameof(Destination)}' value '{Destination}' is invalid. Not a legal path.");
            }

            var fileName = Path.GetFileName(Uri.AbsolutePath);
            var filePath = Path.Combine(Destination, fileName);

            if (_fileSystem.FileExists(filePath))
            {
                if (OverwriteFile)
                {
                    context.Logger.LogDebug("File '{FilePath}' already exists and '{OverwriteFile}' is enabled. Will remove to replace.", filePath, nameof(OverwriteFile));
                    _fileSystem.DeleteFile(filePath);
                }
                else
                {
                    throw new TaskResultsException($"File '{filePath}' already exists.");
                }
            }

            if (!_fileSystem.DirectoryExists(Destination))
            {
                context.Logger.LogDebug("Directory '{Destination}' does not exists. Will create.", Destination);
                _fileSystem.CreateDirectory(Destination);
            }

            context.Logger.LogDebug("Downloading '{Uri}' to '{Path}'...", Uri, filePath);

            context.State = TaskState.Running;

            using var response = await _httpClient.GetAsync(Uri);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            await _fileSystem.AppendFileAsync(filePath, content, context.CancellationToken);
        }
        catch (TaskResultsException e)
        {
            return TaskResults.Failure(e.Message, e.InnerException);
        }
        catch (Exception e)
        {
            return TaskResults.Failure($"An unhandled exception occurred when downloading '{Uri}' to '{Destination}'", e);
        }
        finally
        {
            context.State = TaskState.Completed;
        }

        return TaskResults.Success();
    }
}
