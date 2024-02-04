using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="ITypeAssert"/>
/// for general type assertion sets.
/// </summary>
public interface ITypeAssert
{
    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <param name="expectedType">The type the object should be.</param>
    /// <param name="obj">The object to be evaluated.</param>
    /// <exception cref="AssertException">Thrown when the object is not the given type.</exception>
    void IsType(Type expectedType, [NotNull] object? obj);

    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <param name="obj">The object to be evaluated.</param>
    /// <typeparam name="T">The type the object should be.</typeparam>
    /// <exception cref="AssertException">Thrown when the object is not the given type.</exception>
    void IsType<T>([NotNull] T? obj);
}
