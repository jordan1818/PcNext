using Asys.System.Environment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

namespace Asys.Cli.Framework.System.Internal;

/// <summary>
/// <see cref="IConfigurationProvider"/> based on <see cref="IEnvironmentVariables"/>.
/// </summary>
/// <remarks>
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.EnvironmentVariables/src/EnvironmentVariablesConfigurationProvider.cs"/> for original code.
/// </remarks>
public class CustomEnvironmentVariablesConfigurationProvider : EnvironmentVariablesConfigurationProvider
{
    private const string MySqlServerPrefix = "MYSQLCONNSTR_";
    private const string SqlAzureServerPrefix = "SQLAZURECONNSTR_";
    private const string SqlServerPrefix = "SQLCONNSTR_";
    private const string CustomPrefix = "CUSTOMCONNSTR_";

    private readonly string _prefix;

    private readonly IEnvironmentVariables _environmentVariables;

    /// <summary>
    /// Instantiates a new <see cref="CustomEnvironmentVariablesConfigurationProvider"/>.
    /// </summary>
    /// <param name="environmentVariables">The <see cref="IEnvironmentVariables"/> to use as config source.</param>
    public CustomEnvironmentVariablesConfigurationProvider(IEnvironmentVariables environmentVariables)
    {
        _environmentVariables = environmentVariables;
        _prefix = string.Empty;
    }

    /// <summary>
    /// Instantiates a new <see cref="CustomEnvironmentVariablesConfigurationProvider"/>.
    /// </summary>
    /// <param name="environmentVariables">The <see cref="IEnvironmentVariables"/> to use as config source.</param>
    /// <param name="prefix">A prefix used to filter the environment variables.</param>
    public CustomEnvironmentVariablesConfigurationProvider(IEnvironmentVariables environmentVariables, string? prefix)
        : base(prefix)
    {
        _environmentVariables = environmentVariables;
        _prefix = prefix ?? string.Empty;
    }

    /// <inheritdoc/>
    public override void Load()
    {
        // Get the environment variables from the interface and then loads
        // them using the custom method originally from:
        // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.EnvironmentVariables/src/EnvironmentVariablesConfigurationProvider.cs
        var kvps = _environmentVariables.GetEnvironmentVariables();
        Load(kvps);
    }

    internal void Load(IDictionary<string, string?> envVariables)
    {
        // We could not re-use what the EnvironmentVariablesConfigurationProvider had since it was
        // not opened for extensision, we had to copy/paste most of the code from the original
        // implementation by Microsoft:
        // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.EnvironmentVariables/src/EnvironmentVariablesConfigurationProvider.cs

        var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var e = envVariables.GetEnumerator();

        try
        {
            while (e.MoveNext())
            {
                var entry = e.Current;
                var key = entry.Key;
                string? provider = null;
                string prefix;

#pragma warning disable MA0071 // Avoid using redundant else
                if (key.StartsWith(MySqlServerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    prefix = MySqlServerPrefix;
                    provider = "MySql.Data.MySqlClient";
                }
                else if (key.StartsWith(SqlAzureServerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    prefix = SqlAzureServerPrefix;
                    provider = "System.Data.SqlClient";
                }
                else if (key.StartsWith(SqlServerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    prefix = SqlServerPrefix;
                    provider = "System.Data.SqlClient";
                }
                else if (key.StartsWith(CustomPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    prefix = CustomPrefix;
                }
                else if (key.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
                {
                    // This prevents the prefix from being normalized.
                    // We can also do a fast path branch, I guess? No point in reallocating if the prefix is empty.
                    key = NormalizeKey(key[_prefix.Length..]);
#pragma warning disable CS8601 // Possible null reference assignment.
                    data[key] = entry.Value;
#pragma warning restore CS8601 // Possible null reference assignment.

                    continue;
                }
                else
                {
                    continue;
                }
#pragma warning restore MA0071 // Avoid using redundant else

                // Add the key-value pair for connection string, and optionally provider name
                key = NormalizeKey(key[prefix.Length..]);
#pragma warning disable CS8604 // Possible null reference argument.
                AddIfPrefixed(data, $"ConnectionStrings:{key}", entry.Value);
#pragma warning restore CS8604 // Possible null reference argument.
                if (provider != null)
                {
                    AddIfPrefixed(data, $"ConnectionStrings:{key}_ProviderName", provider);
                }
            }
        }
        finally
        {
            (e as IDisposable)?.Dispose();
        }

        Data = data;
    }

    private void AddIfPrefixed(Dictionary<string, string> data, string key, string value)
    {
        if (key.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
        {
            key = key[_prefix.Length..];
            data[key] = value;
        }
    }

    private static string NormalizeKey(string key) => key.Replace("__", ConfigurationPath.KeyDelimiter, StringComparison.OrdinalIgnoreCase);
}
