using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Asys.Tests.Framework.XUnit.Asserts;

/// <summary>
/// The implementation of <see cref="Asys.Tests.Framework.Asserts.INullAssert"/>
/// for XUnit assertion sets.
/// </summary>
public sealed partial class XUnitAssert
{
    /// <inheritdoc/>
    public void Null([MaybeNull] object? obj) => Assert.Null(obj);

    /// <inheritdoc/>
    public void NotNull([NotNull] object? obj) => Assert.NotNull(obj);

    /// <inheritdoc/>
    public void Null<T>(T? obj) where T : struct => Assert.Null(obj);

    /// <inheritdoc/>
    public void NotNull<T>([NotNull] T? obj) where T : struct => Assert.NotNull(obj);
}
