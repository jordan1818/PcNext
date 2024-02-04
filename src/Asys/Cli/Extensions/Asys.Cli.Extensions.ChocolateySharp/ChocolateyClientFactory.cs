using ChocolateySharp;
using Microsoft.Extensions.Logging;

namespace Asys.Cli.Extensions.ChocolateySharp;

/// <summary>
/// The implementation of <see cref="IChocolateyClientFactory"/> within <see cref="ChocolateyClientFactory"/>.
/// </summary>
public sealed class ChocolateyClientFactory : IChocolateyClientFactory
{
    private readonly IChocolateyClient? _chocolateyClient;

    /// <summary>
    /// Initialize an instance of <see cref="ChocolateyClientFactory"/>.
    /// </summary>
    /// <param name="chocolateyClient">An instance of <see cref="IChocolateyClient"/> to use instead of creating a new instance. Can be null.</param>
    public ChocolateyClientFactory(IChocolateyClient? chocolateyClient)
    {
        _chocolateyClient = chocolateyClient;
    }

    /// <inheritdoc/>
    public IChocolateyClient Create(ILogger logger) => _chocolateyClient ?? new ChocolateyClient(logger);
}
