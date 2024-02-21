namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// The extension for any <see cref="RegistryKey"/> instances.
/// </summary>
public static class RegistryKeyExtensions
{
    /// <summary>Retrieves the specified value within <see cref="RegistryValue"/>.. <b>null</b> is returned if the value doesn't exist.</summary>
    /// <param name="name">Name of value to retrieve.</param>
    /// <param name="expandEnvironmentNames">Whether to expand on the value's environment names. By default it is false.</param>
    /// <returns>The data associated with the value within <see cref="RegistryValue"/>.</returns>
    public static RegistryValue? GetRegistryValue(this IRegistryKey registryKey, string name, bool expandEnvironmentNames = default)
    {
        var value = registryKey.GetValue(name, defaultValue: null, expandEnvironmentNames);
        var valueKind = registryKey.GetValueKind(name);

        return value is not null ? new RegistryValue(value, valueKind ?? RegistryValueKind.None) : null;
    }
}
