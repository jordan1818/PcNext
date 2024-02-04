using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="INullAssert"/>
/// for general null assertion sets.
/// </summary>
public interface INullAssert
{
    /// <summary>
    /// Verifies that an object reference is null.
    /// </summary>
    /// <param name="obj">The object to be inspected</param>
    /// <exception cref="AssertException">Thrown when the object reference is not null.</exception>
    void Null([MaybeNull] object? obj);

    /// <summary>
    /// Verifies that an object reference is not null.
    /// </summary>
    /// <param name="obj">The object to be inspected</param>
    /// <exception cref="AssertException">Thrown when the object reference is null.</exception>
    void NotNull([NotNull] object? obj);

    /// <summary>
    /// Verifies that an object reference is null.
    /// </summary>
    /// <param name="obj">The object to be inspected</param>
    /// <typeparam name="T">The object's type for referencing.</typeparam>
    /// <exception cref="AssertException">Thrown when the object reference is not null.</exception>
    void Null<T>(T? obj)
        where T : struct;

    /// <summary>
    /// Verifies that an object reference is not null.
    /// </summary>
    /// <param name="obj">The object to be inspected.</param>
    /// <typeparam name="T">The object's type for referencing.</typeparam>
    /// <exception cref="AssertException">Thrown when the object reference is null.</exception>
    void NotNull<T>([NotNull] T? obj)
        where T : struct;
}
