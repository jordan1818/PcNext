using Microsoft.Extensions.DependencyInjection.Extensions;
using PcNext.Framework;

namespace System.CommandLine.Builder;

public static class PcNextFrameworkExtensions
{
    /// <summary>
    /// Enables PcNext frameworks defaults.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UsePcNextDefaults(this CommandLineBuilder builder)
    {
        return builder
            .ConfigureServices((context, hostBuilderContext, services) =>
            {
                services.TryAddSingleton<IPcNextConfigurationTaskFactory, PcNextConfigurationTaskFactory>();
                services.TryAddSingleton<ITaskConfigurationTaskFactory, TaskConfigurationTaskFactory>();
                services.TryAddSingleton<ITaskConfigurationTaskFactory, TaskConfigurationTaskFactory>();
            });
    }
}
