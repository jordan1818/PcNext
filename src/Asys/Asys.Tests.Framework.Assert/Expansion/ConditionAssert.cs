using System.Diagnostics.CodeAnalysis;

namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="IConditionAssert"/> implementation.
/// </summary>
public static partial class Assert
{

    /// <summary>
    /// Verifies that an expression is true.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="message">The message to recieve if the condition is invalid.</param>
    /// <exception cref="AssertException">Thrown when the condition is false.</exception>
    public static void True([DoesNotReturnIf(parameterValue: false)] bool condition, string? message = null) => Asserter.True(condition, message);

    /// <summary>
    /// Verifies that an expression is false.
    /// </summary>
    /// <param name="condition">The condition to be inspected.</param>
    /// <param name="message">The message to recieve if the condition is invalid.</param>
    /// <exception cref="AssertException">Thrown when the condition is true.</exception>
    public static void False([DoesNotReturnIf(parameterValue: false)] bool condition, string? message = null) => Asserter.False(condition, message);
}
