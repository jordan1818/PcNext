namespace Asys.System.Environment;

/// <summary>
/// Defines environment variables mutator/accessor.
/// </summary>
public interface IEnvironmentVariables
{
    /// <summary>
    /// Gets the <paramref name="key"/> environment variable.
    /// </summary>
    /// <param name="key">The environment variable key.</param>
    /// <param name="target">The environment variable scope.</param>
    /// <returns>The environment variable value if found, else null.</returns>
    string? GetEnvironmentVariable(string key, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    /// <summary>
    /// Sets the <paramref name="key"/> environment variable.
    /// </summary>
    /// <param name="key">The environment variable key.</param>
    /// <param name="value">The environment variable value.</param>
    /// <param name="target">The environment variable scope.</param>
    void SetEnvironmentVariable(string key, string? value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    /// <summary>
    /// Gets all the environment variable.
    /// </summary>
    /// <param name="target">The environment variables scope.</param>
    /// <returns>The environment variables found.</returns>
    IDictionary<string, string?> GetEnvironmentVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
}
