using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="IEqualAssert"/> implementation.
/// </summary>
public static partial class Assert
{
    /// <summary>
    /// Verifies that two objects are equivalent, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object to be compared against.</param>
    /// <exception cref="AssertException">Thrown when the objects are not equal.</exception>
    public static void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual) => Asserter.Equal(expected, actual);

    /// <summary>
    /// Verifies that two objects are equivalent, using a comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object to be compared against.</param>
    /// <param name="comparer">The comparer object for the object. If null, it'll use the default comparer.</param>
    /// <exception cref="AssertException">Thrown when the objects are not equal.</exception>
    public static void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual, [MaybeNull] IEqualityComparer<T>? comparer = null) => Asserter.Equal(expected, actual, comparer);
}
