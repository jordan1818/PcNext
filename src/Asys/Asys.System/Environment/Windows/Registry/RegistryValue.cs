using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Asys.System.Environment.Windows.Registry;

/// <summary>
/// The definition of <see cref="RegistryValue"/>
/// for value manipulation.
/// </summary>
public sealed class RegistryValue : IEqualityComparer<RegistryValue>
{
    private static readonly Regex _expanedStringRegex = new("%\\w+%", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    /// <summary>
    /// Initializes an instance of <see cref="RegistryValue"/>.
    /// </summary>
    public RegistryValue(object value, RegistryValueKind kind)
    {
        Value = value;
        Kind = kind;
    }

    /// <summary>
    /// The raw value.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// The value's kind.
    /// </summary>
    public RegistryValueKind? Kind { get; }

    /// <summary>
    /// Appends <see cref="Value"/> to another.
    /// </summary>
    /// <param name="left">The left side of the operator that is <see cref="RegistryValue"/>.</param>
    /// <param name="right">The right side of the operator that is <see cref="RegistryValue"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">If <paramref name="left"/> or <paramref name="right"/> are null or <see cref="Value"/> is null.</exception>
    /// <exception cref="ArgumentException">If <paramref name="left"/> and <paramref name="right"/> <see cref="Kind"/> are not equal.</exception>
    /// <exception cref="InvalidOperationException">If the <see cref="RegistryValueKind"/> could not be determined for both <paramref name="left"/> and <paramref name="right"/>. Or not supported.</exception>
    public static RegistryValue operator +(RegistryValue? left, RegistryValue? right)
    {
        if (left is null || left.Value is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right is null || right.Value is null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        if (left.Kind != right.Kind)
        {
            throw new ArgumentException($"Left's kind '{left.Kind}' is not equal to the right '{right.Kind}'. Cannot manipulate.");
        }

        var kind = left.Kind ?? right.Kind 
            ?? DetermineKindFromValue(left.Value) 
            ?? DetermineKindFromValue(right.Value)
            ?? RegistryValueKind.Unknown;

        switch (kind)
        {
            case RegistryValueKind.DWord:
                {

                    var leftIntValue = (int)left.Value;
                    var rightIntValue = (int)right.Value;
                    return new RegistryValue(leftIntValue + rightIntValue, kind);
                }
            case RegistryValueKind.QWord:
                {

                    var leftLongValue = (long)left.Value;
                    var rightLongValue = (long)right.Value;
                    return new RegistryValue(leftLongValue + rightLongValue, kind);
                }
            case RegistryValueKind.MultiString:
                {
                    var leftLongValue = ((string[])left.Value).ToList();
                    var rightLongValue = ((string[])right.Value).ToList();
                    return new RegistryValue(leftLongValue.Union(rightLongValue).ToArray(), kind);
                }
            case RegistryValueKind.ExpandString:
            case RegistryValueKind.String:
                {
                    var leftStringValue = (string)left.Value;
                    var rightStringValue = (string)right.Value;
                    return new RegistryValue(leftStringValue + rightStringValue, kind);
                }
            default: throw new InvalidOperationException($"Unsupported Registry kind '{kind}'.");
        }
    }

    private static RegistryValueKind? DetermineKindFromValue(object? value)
    {
        switch (value)
        {
            case int: return RegistryValueKind.DWord;
            case long: return RegistryValueKind.QWord;
            case string[]: return RegistryValueKind.MultiString;
            case string:
                {
                    if (_expanedStringRegex.IsMatch((string)value!))
                    {
                        return RegistryValueKind.ExpandString;
                    }

                    return RegistryValueKind.String;
                }
            default: return null; // Cannot determine RegistryKind
        }
    }

    private static bool EqualsValue(object? value1, object? value2)
    {
        switch (value1)
        {
            case int: return int.Equals(value1, value2);
            case long: return long.Equals(value1, value2);
            case string: return string.Equals(value1 as string, value2 as string, StringComparison.Ordinal);
            case string[]:
                {
                    var array1 = value1 as string[];
                    var array2 = value2 as string[];
                    return (array1?.OrderBy(m => m) ?? Enumerable.Empty<string>())
                        .SequenceEqual(array2?.OrderBy(m => m) ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
                }
            default: return false; // Cannot evaluate value
        }
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(this, obj as RegistryValue);

    /// <inheritdoc/>
    public override int GetHashCode() => GetHashCode(this);

    /// <inheritdoc/>
    public bool Equals(RegistryValue? x, RegistryValue? y)
        =>
        x is not null && y is not null
        && x.Kind == y.Kind
        && EqualsValue(x.Value, y.Value);

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] RegistryValue obj) => obj.Value?.GetHashCode() ?? 0 + obj.Kind?.GetHashCode() ?? 0;
}
