using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts.Default;

/// <summary>
/// The implementation of <see cref="IEqualAssert"/>
/// for default assertion sets.
/// </summary>
public sealed partial class DefaultAssert
{
    /// <inheritdoc/>
    public void True([DoesNotReturnIf(parameterValue: false)] bool condition, string? message = null)
    {
        if (!condition)
        {
            throw new AssertException(!string.IsNullOrWhiteSpace(message) ? message : $"'{nameof(condition)}' was not as expected. Expected 'True' but was 'False'");
        }
    }

    /// <inheritdoc/>
    public void False([DoesNotReturnIf(parameterValue: true)] bool condition, string? message = null)
    {
        if (condition)
        {
            throw new AssertException(!string.IsNullOrWhiteSpace(message) ? message : $"'{nameof(condition)}' was not as expected. Expected 'False' but was 'True'");
        }
    }
}
