using Asys.System.Convert;

namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// The implemenmtation of <see cref="IRegistryKey"/> within <see cref="RegistryKey"/>.
/// </summary>
public sealed class RegistryKey : IRegistryKey
{
    private readonly Microsoft.Win32.RegistryKey? _registryKey;
    private readonly IRegistryKey? _parent;

    /// <summary>
    /// Initializes an instance of <see cref="RegistryKey"/>.
    /// </summary>
    /// <param name="registryKey">The real <see cref="Microsoft.Win32.RegistryKey"/> to use.</param>
    public RegistryKey(Microsoft.Win32.RegistryKey? registryKey, IRegistryKey? parent = null)
    {
        _registryKey = registryKey;
        _parent = parent;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _registryKey?.Dispose();
        _parent?.Dispose();
    }

    /// <inheritdoc/>
    public IRegistryKey? CreateOrOpenSubKey(string name, RegistryKeyPermissionCheck? permissionCheck = default) => new RegistryKey(_registryKey?.CreateSubKey(name, ConvertEnum.ToOrigin<RegistryKeyPermissionCheck, Microsoft.Win32.RegistryKeyPermissionCheck>(permissionCheck) ?? Microsoft.Win32.RegistryKeyPermissionCheck.Default), this);

    /// <inheritdoc/>
    public object? GetValue(string name, object? defaultValue = default, bool expandEnvironmentNames = default) => _registryKey?.GetValue(name, defaultValue, expandEnvironmentNames ? Microsoft.Win32.RegistryValueOptions.DoNotExpandEnvironmentNames : Microsoft.Win32.RegistryValueOptions.None);

    public RegistryValueKind? GetValueKind(string? name) => ConvertEnum.ToOrigin<Microsoft.Win32.RegistryValueKind, RegistryValueKind>(_registryKey?.GetValueKind(name));

    /// <inheritdoc/>
    public void SetValue(string? name, object value, RegistryValueKind? kind = default) => _registryKey?.SetValue(name, value, ConvertEnum.ToOrigin<RegistryValueKind, Microsoft.Win32.RegistryValueKind>(kind) ?? Microsoft.Win32.RegistryValueKind.Unknown);

    /// <inheritdoc/>
    public void DeleteValue(string name) => _registryKey?.DeleteValue(name);

}
