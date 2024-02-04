using Asys.System.Environment.Windows.Registry;

namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// The implemenmtation of <see cref="IRegistry"/> within <see cref="Registry"/>.
/// </summary>
public sealed class Registry : IRegistry
{
    /// <inheritdoc/>
    public IRegistryKey ClassesRoot => new RegistryKey(Microsoft.Win32.Registry.ClassesRoot);

    /// <inheritdoc/>
    public IRegistryKey CurrentUser => new RegistryKey(Microsoft.Win32.Registry.CurrentUser);

    /// <inheritdoc/>
    public IRegistryKey LocalMachine => new RegistryKey(Microsoft.Win32.Registry.LocalMachine);

    /// <inheritdoc/>
    public IRegistryKey Users => new RegistryKey(Microsoft.Win32.Registry.Users);

    /// <inheritdoc/>
    public IRegistryKey PerformanceData => new RegistryKey(Microsoft.Win32.Registry.PerformanceData);

    /// <inheritdoc/>
    public IRegistryKey CurrentConfig => new RegistryKey(Microsoft.Win32.Registry.CurrentConfig);

    /// <inheritdoc/>
    public IRegistryKey? Get(RegistryHive hive)
    {
        return hive switch
        {
            RegistryHive.ClassesRoot => ClassesRoot,
            RegistryHive.CurrentUser => CurrentUser,
            RegistryHive.LocalMachine => LocalMachine,
            RegistryHive.Users => Users,
            RegistryHive.PerformanceData => PerformanceData,
            RegistryHive.CurrentConfig => CurrentConfig,
            _ => throw new NotSupportedException($"'{nameof(RegistryHive)}' value '{hive}' is not supported."),
        };
    }
}
