namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="AssertException"/>
/// for <see cref="IAssert"/> exceptions.
/// </summary>
public class AssertException : Exception
{
    /// <summary>
    /// Intialize an instance of <see cref="AssertException"/>.
    /// </summary>
    /// <param name="message">The message of the assertion that has occurred.</param>
    public AssertException(string? message)
        : base(message)
    {

    }
}
