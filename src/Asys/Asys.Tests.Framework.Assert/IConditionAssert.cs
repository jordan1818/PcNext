using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="IConditionAssert"/>
/// for general condition assertion sets.
/// </summary>
public interface IConditionAssert
{
    /// <summary>
    /// Verifies that an expression is true.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="message">The message to recieve if the condition is invalid.</param>
    /// <exception cref="AssertException">Thrown when the condition is false.</exception>
    void True([DoesNotReturnIf(parameterValue: false)] bool condition, string? message = null);

    /// <summary>
    /// Verifies that an expression is false.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="message">The message to recieve if the condition is invalid.</param>
    /// <exception cref="AssertException">Thrown when the condition is true.</exception>
    void False([DoesNotReturnIf(parameterValue: true)] bool condition, string? message = null);
}
