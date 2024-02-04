using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="INullAssert"/> implementation.
/// </summary>
public static partial class Assert
{
    /// <summary>
    /// Verifies that an object reference is null.
    /// </summary>
    /// <param name="obj">The object to be inspected</param>
    /// <exception cref="AssertException">Thrown when the object reference is not null.</exception>
    public static void Null([MaybeNull] object? obj) => Asserter.Null(obj);

    /// <summary>
    /// Verifies that an object reference is not null.
    /// </summary>
    /// <param name="obj">The object to be inspected</param>
    /// <exception cref="AssertException">Thrown when the object reference is null.</exception>
    public static void NotNull([NotNull] object? obj) => Asserter.NotNull(obj);

    /// <summary>
    /// Verifies that an object reference is null.
    /// </summary>
    /// <param name="obj">The object to be inspected</param>
    /// <typeparam name="T">The object's type for referencing.</typeparam>
    /// <exception cref="AssertException">Thrown when the object reference is not null.</exception>
    public static void Null<T>(T? obj) where T : struct => Asserter.Null(obj);

    /// <summary>
    /// Verifies that an object reference is not null.
    /// </summary>
    /// <param name="obj">The object to be inspected.</param>
    /// <typeparam name="T">The object's type for referencing.</typeparam>
    /// <exception cref="AssertException">Thrown when the object reference is null.</exception>
    public static void NotNull<T>([NotNull] T? obj) where T : struct => Asserter.NotNull(obj);
}