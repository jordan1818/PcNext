using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="IEqualAssert"/>
/// for general equal assertion sets.
/// </summary>
public interface IEqualAssert
{
    /// <summary>
    /// Verifies that two objects are equivalent, using a default comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object to be compared against.</param>
    /// <exception cref="AssertException">Thrown when the objects are not equal.</exception>
    void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual);

    /// <summary>
    /// Verifies that two objects are equivalent, using a comparer.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be compared.</typeparam>
    /// <param name="expected">The expected object.</param>
    /// <param name="actual">The actual object to be compared against.</param>
    /// <param name="comparer">The comparer object for the object. If null, it'll use the default comparer.</param>
    /// <exception cref="AssertException">Thrown when the objects are not equal.</exception>
    void Equal<T>([AllowNull] T? expected, [AllowNull] T? actual, [MaybeNull] IEqualityComparer<T>? comparer = null);
}
