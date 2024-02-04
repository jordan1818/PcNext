using Asys.System.Environment;
using Asys.System.Environment.Windows.Registry;
using Asys.System.Environment.Windows.Scheduler;
using Asys.System.Security;
using Asys.Tasks;
using Asys.Tasks.Process;
using Asys.Cli.Extensions.Asys.System.IO;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;

namespace PcNext.Framework;

internal sealed class TaskConfigurationTaskFactory : ITaskConfigurationTaskFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IProcessTaskFactory _processTaskFactory;
    private readonly IFileSystemFactory _fileSystemFactory;
    private readonly IOperatingSystem _operatingSystem;
    private readonly IAccountManager _accountManager;
    private readonly IRegistry _registry;
    private readonly ITaskScheduler _taskScheduler;

    public TaskConfigurationTaskFactory(IHttpClientFactory httpClientFactory, IProcessTaskFactory processTaskFactory, IFileSystemFactory fileSystemFactory, IOperatingSystem operatingSystem, IAccountManager accountManager, IRegistry registry, ITaskScheduler taskScheduler)
    {
        _httpClientFactory = httpClientFactory;
        _processTaskFactory = processTaskFactory;
        _fileSystemFactory = fileSystemFactory;
        _operatingSystem = operatingSystem;
        _accountManager = accountManager;
        _registry = registry;
        _taskScheduler = taskScheduler;
    }

    public ITask Create(TaskConfiguration taskConfiguration)
    {
        return taskConfiguration.Type switch
        {
            TaskType.Cmd => new ShellTask(taskConfiguration, ShellType.Cmd, _processTaskFactory),
            TaskType.PowerShell => new ShellTask(taskConfiguration, ShellType.PowerShell, _processTaskFactory),
            TaskType.Registry => new RegistryTask(taskConfiguration, _registry, _operatingSystem),
            TaskType.OnStartUp => new OnStartUpTask(taskConfiguration, _taskScheduler, _operatingSystem, _accountManager),
            TaskType.Download => new DownloadTask(taskConfiguration, _httpClientFactory.CreateClient(), _fileSystemFactory.Create()),
            TaskType.DownloadAndInstall => new DownloadAndInstallTask(taskConfiguration, _fileSystemFactory.Create(), this, _processTaskFactory),
            _ => throw new NotSupportedException($"Task type '{taskConfiguration.Type}' is not support from task '{taskConfiguration.Name}'."),
        };
    }
}
