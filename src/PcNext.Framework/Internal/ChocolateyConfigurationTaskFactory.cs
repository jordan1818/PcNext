using Asys.Tasks;
using Asys.Cli.Extensions.ChocolateySharp;
using Microsoft.Extensions.Logging;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;

namespace PcNext.Framework.Internal;

internal class ChocolateyConfigurationTaskFactory : IChocolateyConfigurationTaskFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IChocolateyClientFactory _chocolateyClientFactory;
    private readonly ITaskConfigurationTaskFactory _taskConfigurationTaskFactory;

    public ChocolateyConfigurationTaskFactory(ILoggerFactory loggerFactory, IChocolateyClientFactory chocolateyClientFactory, ITaskConfigurationTaskFactory taskConfigurationTaskFactory)
    {
        _loggerFactory = loggerFactory;
        _chocolateyClientFactory = chocolateyClientFactory;
        _taskConfigurationTaskFactory = taskConfigurationTaskFactory;
    }

    public ITask Create(ChocolateyConfiguration chocolateyConfiguration) => new ChocolateyTask(chocolateyConfiguration, _chocolateyClientFactory.Create(_loggerFactory.CreateLogger(nameof(ChocolateyTask))), _taskConfigurationTaskFactory);
}
