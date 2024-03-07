using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.XUnit.Asserts;

using Assert = Xunit.Assert;

/// <summary>
/// The implementation of <see cref="Asys.Tests.Framework.Asserts.IEqualAssert"/>
/// for XUnit assertion sets.
/// </summary>
public sealed partial class XUnitAssert
{
    public void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual) => Assert.Equal(expected, actual);

    /// <inheritdoc/>
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    public void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual, [MaybeNull] IEqualityComparer<T>? comparer = null) => Assert.Equal(expected, actual, comparer ?? EqualityComparer<T>.Default);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
}
