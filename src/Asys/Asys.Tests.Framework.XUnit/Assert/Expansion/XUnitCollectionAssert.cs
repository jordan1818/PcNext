using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Asys.Tests.Framework.XUnit.Asserts;

/// <summary>
/// The implementation of <see cref="Asys.Tests.Framework.Asserts.ICollectionAssert"/>
/// for XUnit assertion sets.
/// </summary>
public sealed partial class XUnitAssert
{
    /// <inheritdoc/>
    public void Contains<T>(T? expected, IEnumerable<T?> collection) => Assert.Contains(expected, collection);

    /// <inheritdoc/>
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    public void Contains<T>(T? expected, IEnumerable<T?> collection, [MaybeNull] IEqualityComparer<T>? comparer = null) => Assert.Contains(expected, collection, comparer ?? EqualityComparer<T>.Default);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
}
