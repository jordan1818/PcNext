namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// Specifies whether security checks are performed when opening registry keys and accessing their name/value pairs.
/// Based off of <see cref="Microsoft.Win32.RegistryKeyPermissionCheck"/>.
/// </summary>
public enum RegistryKeyPermissionCheck
{
    /// <summary>
    /// The registry key inherits the mode of its parent. Security checks are performed
    /// when trying to access subkeys or values, unless the parent was opened with <see cref="ReadSubTree"/>
    /// or <see cref="ReadWriteSubTree"/> mode.
    /// </summary>
    Default,

    /// <summary>
    /// Security checks are not performed when accessing subkeys or values. A security
    /// check is performed when trying to open the current key, unless the parent was
    /// opened with <see cref="ReadSubTree"/> or <see cref="ReadWriteSubTree"/>
    /// </summary>
    ReadSubTree,

    /// <summary>
    /// Security checks are not performed when accessing subkeys or values. A security
    /// check is performed when trying to open the current key, unless the parent was
    /// opened with <see cref="ReadWriteSubTree"/>
    /// </summary>
    ReadWriteSubTree,
}
