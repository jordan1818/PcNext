namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// Represents the possible values for a top-level node on a machine.
/// Base off of <see cref="Microsoft.Win32.RegistryHive"/>.
/// </summary>
public enum RegistryHive
{
    /// <summary>
    /// Represents the HKEY_CLASSES_ROOT base key.
    /// </summary>
    ClassesRoot,

    /// <summary>
    /// Represents the HKEY_CURRENT_USER base key.
    /// </summary>
    CurrentUser,

    /// <summary>
    /// Represents the HKEY_LOCAL_MACHINE base key.
    /// </summary>
    LocalMachine,

    /// <summary>
    /// Represents the HKEY_USERS base key.
    /// </summary>
    Users,

    /// <summary>
    /// Represents the HKEY_PERFORMANCE_DATA base key.
    /// </summary>
    PerformanceData,

    /// <summary>
    /// Represents the HKEY_CURRENT_CONFIG base key.
    /// </summary>
    CurrentConfig,
}
