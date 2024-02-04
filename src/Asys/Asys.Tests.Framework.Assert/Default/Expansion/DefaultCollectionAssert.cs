using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts.Default;

/// <summary>
/// The implementation of <see cref="ICollectionAssert"/>
/// for default assertion sets.
/// </summary>
public sealed partial class DefaultAssert
{
    /// <inheritdoc/>
    public void Contains<T>(T? expected, IEnumerable<T?> collection) => Contains(expected, collection, comparer: null);

    /// <inheritdoc/>
    public void Contains<T>(T? expected, IEnumerable<T?> collection, [MaybeNull] IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        if (!collection.Contains(expected, comparer))
        {
            throw new AssertException("");
        }
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    }
}
