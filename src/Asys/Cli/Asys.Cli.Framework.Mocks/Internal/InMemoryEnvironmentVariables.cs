using Asys.System.Environment;
using EnvironmentVariableTarget = Asys.System.Environment.EnvironmentVariableTarget;

namespace Asys.Cli.Framework.Mocks.Internal;

public class InMemoryEnvironmentVariables : IEnvironmentVariables
{
    private readonly IDictionary<EnvironmentVariableTarget, IDictionary<string, string?>> _inMemoryEnvironmentVariables;

    public InMemoryEnvironmentVariables(IEnvironmentVariables? realEnvironmentVariables = null)
    {
        _inMemoryEnvironmentVariables = new Dictionary<EnvironmentVariableTarget, IDictionary<string, string?>>
        {
            [EnvironmentVariableTarget.Machine] = realEnvironmentVariables?.GetEnvironmentVariables(EnvironmentVariableTarget.Machine) ?? new Dictionary<string, string?>(),
            [EnvironmentVariableTarget.User] = realEnvironmentVariables?.GetEnvironmentVariables(EnvironmentVariableTarget.User) ?? new Dictionary<string, string?>(),
            [EnvironmentVariableTarget.Process] = realEnvironmentVariables?.GetEnvironmentVariables(EnvironmentVariableTarget.Process) ?? new Dictionary<string, string?>(),
        };
    }

    public string? GetEnvironmentVariable(string key, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process) => _inMemoryEnvironmentVariables[target][key];

    public IDictionary<string, string?> GetEnvironmentVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process) => _inMemoryEnvironmentVariables[target];

    public void SetEnvironmentVariable(string key, string? value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process) => _inMemoryEnvironmentVariables[target][key] = value;
}
