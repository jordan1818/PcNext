using Microsoft.Extensions.Configuration;

namespace Asys.Cli.Framework.System.Internal;

using Asys.System.Environment;

/// <summary>
/// <see cref="IConfigurationSource"/> based on <see cref="IEnvironmentVariables"/>.
/// </summary>
/// <remarks>
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.EnvironmentVariables/src/EnvironmentVariablesConfigurationSource.cs"/> for original code.
/// </remarks>
public class CustomEnvironmentVariablesConfigurationSource : IConfigurationSource
{
    private readonly IEnvironmentVariables _environmentVariables;

    /// <summary>
    /// A prefix used to filter environment variables.
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Instantiates a new <see cref="CustomEnvironmentVariablesConfigurationSource"/>.
    /// </summary>
    /// <param name="environmentVariables">The <see cref="IEnvironmentVariables"/> to use as config source.</param>
    public CustomEnvironmentVariablesConfigurationSource(IEnvironmentVariables environmentVariables)
    {
        _environmentVariables = environmentVariables;
    }

    /// <summary>
    /// Builds the <see cref="CustomEnvironmentVariablesConfigurationProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>A <see cref="CustomEnvironmentVariablesConfigurationProvider"/></returns>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CustomEnvironmentVariablesConfigurationProvider(_environmentVariables, Prefix);
    }
}
