namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// Specifies the data types to use when storing values in the registry, or identifies
/// the data type of a value in the registry.
/// Based off of <see cref="Microsoft.Win32.RegistryValueKind"/>.
/// </summary>
public enum RegistryValueKind
{
    /// <summary>
    /// No data type.
    /// </summary>
    None,

    /// <summary>
    /// An unsupported registry data type. For example, the Microsoft Windows API registry
    /// data type REG_RESOURCE_LIST is unsupported. Use this value to specify that the
    /// <see cref="IRegistryKey.SetValue(string?, object)"/> method should
    /// determine the appropriate registry data type when storing a name/value pair.
    /// </summary>
    Unknown,

    /// <summary>
    /// A null-terminated string. This value is equivalent to the Windows API registry
    /// data type REG_SZ.
    /// </summary>
    String,

    /// <summary>
    /// A null-terminated string that contains unexpanded references to environment variables,
    /// such as %PATH%, that are expanded when the value is retrieved. This value is
    /// equivalent to the Windows API registry data type REG_EXPAND_SZ.
    /// </summary>
    ExpandString,

    /// <summary>
    /// Binary data in any form. This value is equivalent to the Windows API registry
    /// data type REG_BINARY.
    /// </summary>
    Binary,

    /// <summary>
    /// A 32-bit binary number. This value is equivalent to the Windows API registry
    /// data type REG_DWORD.
    /// </summary>
    DWord,

    /// <summary>
    /// An array of null-terminated strings, terminated by two null characters. This
    /// value is equivalent to the Windows API registry data type REG_MULTI_SZ.
    /// </summary>
    MultiString,

    /// <summary>
    /// A 64-bit binary number. This value is equivalent to the Windows API registry
    /// data type REG_QWORD.
    /// </summary>
    QWord,
}