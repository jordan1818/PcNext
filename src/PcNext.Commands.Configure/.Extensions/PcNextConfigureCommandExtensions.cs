using PcNext.Commands.Configure;

namespace System.CommandLine.Builder;

public static class PcNextConfigureCommandExtensions
{
    /// <summary>
    /// Enables PcNext configuration intergrations.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UsePcNextConfigureIntegrations(this CommandLineBuilder builder)
        => builder
            .ConfigureCommands(root => root.AddCommand<ConfigureCommand>());
}
