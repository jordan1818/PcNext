namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// The definition of <see cref="IRegistryKey"/>.
/// Based off of <see cref="Microsoft.Win32.RegistryKey"/>.
/// </summary>
public interface IRegistryKey : IDisposable
{
    /// <summary>
    /// Creates a new subkey, or opens an existing one.
    /// </summary>
    /// <param name="name">Name or path to subkey to create or open.</param>
    /// <param name="permissionCheck"></param>
    /// <returns>The subkey, or <b>null</b> if the operation failed.</returns>
    IRegistryKey? CreateOrOpenSubKey(string name, RegistryKeyPermissionCheck? permissionCheck = default);

    /// <summary>Retrieves the specified value. <i>defaultValue</i> is returned if the value doesn't exist.</summary>
    /// <remarks>
    /// Note that <var>name</var> can be null or "", at which point the
    /// unnamed or default value of this Registry key is returned, if any.
    /// The default values for RegistryKeys are OS-dependent.  NT doesn't
    /// have them by default, but they can exist and be of any type.  On
    /// Win95, the default value is always an empty key of type REG_SZ.
    /// Win98 supports default values of any type, but defaults to REG_SZ.
    /// </remarks>
    /// <param name="name">Name of value to retrieve.</param>
    /// <param name="defaultValue">Value to return if <i>name</i> doesn't exist.</param>
    /// <param name="expandEnvironmentNames">Whether to expand on the value's environment names. By default it is false.</param>
    /// <returns>The data associated with the value.</returns>
    object? GetValue(string name, object? defaultValue = default, bool expandEnvironmentNames = default);

    /// <summary>Retrieves the specified value's kind.</summary>
    /// <param name="name"></param>
    /// <returns>Returns the value's <see cref="RegistryValueKind"/>, or <b>null</b> if could not be found.</returns>
    RegistryValueKind? GetValueKind(string? name);

    /// <summary>
    /// Sets the specified value.
    /// </summary>
    /// <param name="name">Name of value to retrieve.</param>
    /// <param name="value">Data to store.</param>
    /// <param name="kind">The data's type to be stored as. By default, it is using the default kind.</param>
    void SetValue(string? name, object value, RegistryValueKind? kind = default);

    /// <summary>
    /// Deletes the specified value from this key.
    /// </summary>
    /// <param name="name">Name of value to delete.</param>
    void DeleteValue(string name);
}
