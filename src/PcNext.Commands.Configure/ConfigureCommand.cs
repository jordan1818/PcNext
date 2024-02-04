using Asys.Tasks.Contexts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PcNext.Framework;
using PcNext.Framework.Configurations;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace PcNext.Commands.Configure;

internal sealed class ConfigureCommand : Command<ConfigureCommand.CommandHandler>
{
    public ConfigureCommand() 
        : base("configure", "Installs and configures, by the provided configuration, for the current personal computer (PC).")
    {
        var failOption = new Option<bool>($"--fail-on-error", "A switch that will fail on the first error from installing/configuring.");

        AddOption(failOption);
    }

    internal sealed class CommandHandler : CommandHandlerBase
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IPcNextConfigurationTaskFactory _pcNextConfigurationTaskFactory;
        private readonly PcNextConfiguration _pcNextConfiguration;

        public CommandHandler(ILogger<CommandHandler> logger, IOptions<PcNextConfiguration> pcNextConfiguration, IPcNextConfigurationTaskFactory pcNextConfigurationTaskFactory)
        {
            _logger = logger;
            _pcNextConfiguration = pcNextConfiguration.Value;
            _pcNextConfigurationTaskFactory = pcNextConfigurationTaskFactory;
        }

        public bool FailOnError { get; set; }

        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            var cancellationToken = context.GetCancellationToken();

            var configureTasks = _pcNextConfigurationTaskFactory.Create(_pcNextConfiguration);
            if (!configureTasks.Any())
            {
                _logger.LogDebug("No configuration tasks has been found. Skipping execution...");
            }

            foreach (var configureTask in configureTasks)
            {
                var taskContext = new TaskContext(_logger)
                {
                    CancellationToken = cancellationToken,
                };

                var taskResults = await configureTask.ExecuteAsync(taskContext);
                if (!taskResults.IsSuccessful)
                {
                    var failureMessage = $"A task has failed with message '{taskResults.FailureMessage}' and/or with exception '{taskResults.Exception}'";
                    if (!FailOnError)
                    {
                        _logger.LogWarning(failureMessage);
                    }
                    else
                    {
                        _logger.LogCritical(failureMessage);
                        return 1;
                    }
                }
            }

            return 0;
        }
    }
}