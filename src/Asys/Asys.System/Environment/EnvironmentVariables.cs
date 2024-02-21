using Asys.System.Convert;
using System.Collections;

using SystemEnvironment = System.Environment;
using SystemEnvironmentVariableTarget = System.EnvironmentVariableTarget;

namespace Asys.System.Environment;

/// <summary>
/// <see cref="SystemEnvironment"/> implementation of <see cref="IEnvironmentVariables"/>.
/// </summary>
public sealed class EnvironmentVariables : IEnvironmentVariables
{
    /// <inheritdoc/>
    public string? GetEnvironmentVariable(string key, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process) => SystemEnvironment.GetEnvironmentVariable(key, ConvertEnum.ToOrigin<EnvironmentVariableTarget, SystemEnvironmentVariableTarget>(target) ?? SystemEnvironmentVariableTarget.Process);

    /// <inheritdoc/>
    public void SetEnvironmentVariable(string key, string? value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process) => SystemEnvironment.SetEnvironmentVariable(key, value, ConvertEnum.ToOrigin<EnvironmentVariableTarget, SystemEnvironmentVariableTarget>(target) ?? SystemEnvironmentVariableTarget.Process);

    /// <inheritdoc/>
    public IDictionary<string, string?> GetEnvironmentVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        var environmentVariables = SystemEnvironment.GetEnvironmentVariables(ConvertEnum.ToOrigin<EnvironmentVariableTarget, SystemEnvironmentVariableTarget>(target) ?? SystemEnvironmentVariableTarget.Process);
        var output = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (DictionaryEntry entry in environmentVariables)
        {
            var key = (string)entry.Key;
            var value = entry.Value as string;

            output[key] = value;
        }

        return output;
    }
}
