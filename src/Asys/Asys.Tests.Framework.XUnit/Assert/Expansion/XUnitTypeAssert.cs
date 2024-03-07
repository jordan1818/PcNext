using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.XUnit.Asserts;

using Assert = Xunit.Assert;

/// <summary>
/// The implementation of <see cref="Asys.Tests.Framework.Asserts.ITypeAssert"/>
/// for XUnit assertion sets.
/// </summary>
public sealed partial class XUnitAssert
{
    /// <inheritdoc/>
    public void IsType(Type expectedType, [NotNull] object? obj) => Assert.IsType(expectedType, obj);

    /// <inheritdoc/>
    public void IsType<T>([NotNull] T? obj) => Assert.IsType<T>(obj);
}
