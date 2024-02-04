using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="ITypeAssert"/> implementation.
/// </summary>
public static partial class Assert
{
    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <param name="expectedType">The type the object should be.</param>
    /// <param name="obj">The object to be evaluated.</param>
    /// <exception cref="AssertException">Thrown when the object is not the given type.</exception>
    public static void IsType([NotNull] Type expectedType, object? obj) => Asserter.IsType(expectedType, obj);

    /// <summary>
    /// Verifies that an object is exactly the given type (and not a derived type).
    /// </summary>
    /// <param name="obj">The object to be evaluated.</param>
    /// <typeparam name="T">The type the object should be.</typeparam>
    /// <exception cref="AssertException">Thrown when the object is not the given type.</exception>
    public static void IsType<T>([NotNull] T? obj) => Asserter.IsType(obj);
}
