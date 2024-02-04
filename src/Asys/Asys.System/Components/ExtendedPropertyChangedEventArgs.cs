using System.ComponentModel;

namespace Asys.System.Components;

/// <summary>
/// Extended definition of <see cref="PropertyChangedEventArgs"/>.
/// </summary>
public class ExtendedPropertyChangedEventArgs : PropertyChangedEventArgs
{
    /// <summary>
    /// Initializes an instance of <see cref="ExtendedPropertyChangedEventArgs"/>
    /// </summary>
    /// <param name="propertyName">The property name. Passed to <see cref="PropertyChangedEventArgs"/>.</param>
    /// <param name="propertyType">The property <see cref="Type"/> that has been changed.</param>
    /// <param name="oldValue">The old value of <paramref name="propertyName"/>.</param>
    /// <param name="newValue">The new value of <paramref name="propertyName"/>.</param>
    public ExtendedPropertyChangedEventArgs(string propertyName, Type? propertyType = null, object? oldValue = default, object? newValue = default)
        : base(propertyName)
    {
        PropertyType = propertyType;
        OldValue = oldValue;
        NewValue = newValue;
    }

    /// <summary>
    /// The property <see cref="Type"/> that has been changed.
    /// </summary>
    public Type? PropertyType { get; }

    /// <summary>
    /// The old value of <see cref="PropertyChangedEventArgs.PropertyName"/>.
    /// </summary>
    public object? OldValue { get; }

    /// <summary>
    /// The new value of <see cref="PropertyChangedEventArgs.PropertyName"/>.
    /// </summary>
    public object? NewValue { get; }
}
