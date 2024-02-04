using ChocolateySharp;
using Asys.Cli.Extensions.ChocolateySharp;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace System.CommandLine.Builder;

/// <summary>
/// Enables the default CLI ChocolateySharp behaviors for <paramref name="builder"/>.
/// </summary>
public static class CliChocolateySharpExtensios
{
    /// <summary>
    /// Enables ChocolateySharp services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseChocolateySharp(this CommandLineBuilder builder)
    {
        return builder
            .ConfigureServices((context, hostBuilderContext, services) =>
            {
                services.TryAddSingleton<IChocolateyClientFactory, ChocolateyClientFactory>();
                services.TryAddSingleton<IChocolateyClient, ChocolateyClient>();
            });
    }

}