using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Asys.Tests.Framework.Asserts.Default;

/// <summary>
/// The implementation of <see cref="INullAssert"/>
/// for default assertion sets.
/// </summary>
public sealed partial class DefaultAssert
{
    /// <inheritdoc/>
    public void Null([MaybeNull] object? obj)
    {
        if (obj is not null)
        {
            throw new AssertException($"'{nameof(obj)}' of type '{obj.GetType().GetTypeInfo().Assembly.GetName().FullName}' is not null but expected null.");
        }
    }

    /// <inheritdoc/>
    public void NotNull([NotNull] object? obj)
    {
        if (obj is null)
        {
            throw new AssertException($"'{nameof(obj)}' is null but expected not null.");
        }
    }

    /// <inheritdoc/>
    public void Null<T>(T? obj) where T : struct
    {
        if (obj is not null)
        {
            throw new AssertException($"'{nameof(obj)}' of type '{typeof(T).GetType().GetTypeInfo().Assembly.GetName().FullName}' is not null but expected null.");
        }
    }

    /// <inheritdoc/>
    public void NotNull<T>([NotNull] T? obj) where T : struct
    {
        if (obj is null)
        {
            throw new AssertException($"'{nameof(obj)}' of type '{typeof(T).GetTypeInfo().Assembly.GetName().FullName}' is null but expected not null.");
        }
    }
}
