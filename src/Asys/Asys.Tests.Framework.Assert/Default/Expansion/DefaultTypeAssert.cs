using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Asys.Tests.Framework.Asserts.Default;

/// <summary>
/// The implementation of <see cref="ITypeAssert"/>
/// for default assertion sets.
/// </summary>
public sealed partial class DefaultAssert
{
    /// <inheritdoc/>
    public void IsType(Type expectedType, [NotNull] object? obj)
    {
        NotNull(obj);

        var actualType = obj.GetType();
        if (expectedType != actualType)
        {
            throw new AssertException($"'{nameof(obj)}' of type '{actualType.GetTypeInfo().Assembly.GetName().FullName}' is not as expected type '{expectedType.GetTypeInfo().Assembly.GetName().FullName}'.");
        }
    }

    /// <inheritdoc/>
    public void IsType<T>([NotNull] T? obj) => IsType(typeof(T), obj);
}
