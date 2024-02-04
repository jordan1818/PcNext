using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Asys.Tests.Framework.Asserts.Default;

/// <summary>
/// The implementation of <see cref="IEqualAssert"/>
/// for default assertion sets.
/// </summary>
public sealed partial class DefaultAssert
{
    /// <inheritdoc/>
    public void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual) => Equal(expected, actual, comparer: null);

    /// <inheritdoc/>
    public void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual, [MaybeNull] IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (expected is null && actual is null)
        {
            return;
        }

        if (!comparer.Equals(expected, actual))
        {
            throw new AssertException($"'{nameof(expected)}' is not equal to '{nameof(actual)}' of type '{typeof(T).GetTypeInfo().Assembly.GetName().FullName}'.");
        }
    }
}
