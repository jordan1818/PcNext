namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// The defintion of <see cref="IRegistry"/>.
/// Base off of <see cref="Microsoft.Win32.Registry"/>.
/// </summary>
public interface IRegistry
{
    /// <summary>
    /// Classes Root Key. This is the root key of class information.
    /// </summary>
    IRegistryKey ClassesRoot { get; }

    /// <summary>
    /// Current User Key. This key should be used as the root for all user specific settings.
    /// </summary>
    IRegistryKey CurrentUser { get; }

    /// <summary>
    /// Local Machine key. This key should be used as the root for all machine specific settings.
    /// </summary>
    IRegistryKey LocalMachine { get; }

    /// <summary>
    /// Users Root Key. This is the root of users.
    /// </summary>
    IRegistryKey Users { get; }

    /// <summary>
    /// Performance Root Key. This is where dynamic performance data is stored on NT.
    /// </summary>
    IRegistryKey PerformanceData { get; }

    /// <summary>
    /// Current Config Root Key. This is where current configuration information is stored.
    /// </summary>
    IRegistryKey CurrentConfig { get; }

    /// <summary>
    /// Gets an instance of <see cref="IRegistryKey"/> from the
    /// appropriate <see cref="RegistryHive"/>.
    /// </summary>
    /// <param name="hive">The hive to return.</param>
    /// <returns>An instance of <see cref="IRegistryKey"/> from <paramref name="hive"/>.</returns>
    IRegistryKey? Get(RegistryHive hive);
}
