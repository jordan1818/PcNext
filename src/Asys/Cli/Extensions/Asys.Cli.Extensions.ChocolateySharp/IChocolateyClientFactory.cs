using ChocolateySharp;
using Microsoft.Extensions.Logging;

namespace Asys.Cli.Extensions.ChocolateySharp;

/// <summary>
/// A definition of <see cref="IChocolateyClientFactory"/> which creates <see cref="IChocolateyClient"/>.
/// </summary>
public interface IChocolateyClientFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IChocolateyClient"/>.
    /// </summary>
    /// <param name="logger">The logger for the <see cref="IChocolateyClient"/> will use.</param>
    /// <returns>An instance of a implemented <see cref="IChocolateyClient"/>.</returns>
    IChocolateyClient Create(ILogger logger); 
}
