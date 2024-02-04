using Asys.Tasks;
using Microsoft.Extensions.Logging;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal;

namespace PcNext.Framework;

internal sealed class PcNextConfigurationTaskFactory : IPcNextConfigurationTaskFactory
{
    private readonly ILogger _logger;
    private readonly IChocolateyConfigurationTaskFactory _chocolateyConfigurationTaskFactory;
    private readonly ITaskConfigurationTaskFactory _taskConfigurationTaskFactory;

    public PcNextConfigurationTaskFactory(ILogger logger, IChocolateyConfigurationTaskFactory chocolateyConfigurationTaskFactory, ITaskConfigurationTaskFactory taskConfigurationTaskFactory)
    {
        _logger = logger;
        _chocolateyConfigurationTaskFactory = chocolateyConfigurationTaskFactory;
        _taskConfigurationTaskFactory = taskConfigurationTaskFactory;
    }

    public IEnumerable<ITask> Create(PcNextConfiguration pcNextConfiguration)
    {
        _logger.LogDebug("'{PcNextConfigurationName}' of '{ConfiguratioName}' contains '{ConfigurationCount}' tasks.", nameof(PcNextConfiguration), nameof(PcNextConfiguration.Chocolatey), pcNextConfiguration.Chocolatey.Count());
        foreach (var chocolateyConfiguration in pcNextConfiguration.Chocolatey)
        {
            yield return _chocolateyConfigurationTaskFactory.Create(chocolateyConfiguration);
        }

        _logger.LogDebug("'{PcNextConfigurationName}' of '{ConfiguratioName}' contains '{ConfigurationCount}' tasks.", nameof(PcNextConfiguration), nameof(PcNextConfiguration.AdditionalTasks), pcNextConfiguration.AdditionalTasks.Count());
        foreach (var taskConfiguration in pcNextConfiguration.AdditionalTasks)
        {
            yield return _taskConfigurationTaskFactory.Create(taskConfiguration);
        }
    }
}
