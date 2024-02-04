using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Asys.Tests.Framework.XUnit.Asserts;

/// <summary>
/// The implementation of <see cref="Asys.Tests.Framework.Asserts.IConditionAssert"/>
/// for XUnit assertion sets.
/// </summary>
public sealed partial class XUnitAssert
{
    /// <inheritdoc/>
    public void True([DoesNotReturnIf(parameterValue: false)] bool condition, string? message = null) => Assert.True(condition, message);

    /// <inheritdoc/>
    public void False([DoesNotReturnIf(parameterValue: true)] bool condition, string? message = null) => Assert.False(condition, message);
}
