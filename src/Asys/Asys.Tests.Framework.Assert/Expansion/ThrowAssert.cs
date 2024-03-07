namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// A static assertion set
/// for <see cref="IThrowAssert"/> implementation.
/// </summary>
public static partial class Assert
{
    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <param name="exceptionType">The type of the exception expected to be thrown.</param>
    /// <param name="action">A delegate to the code to be tested.</param>
    public static void Throws(Type exceptionType, Action action) => Asserter.Throws(exceptionType, action);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TException">The type of the exception expected to be thrown.</typeparam>
    /// <param name="action">A delegate to the code to be tested.</param>
    public static void Throws<TException>(Action action) where TException : Exception => Asserter.Throws<TException>(action);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type) asynchronously.
    /// </summary>
    /// <param name="exceptionType">The type of the exception expected to be thrown.</param>
    /// <param name="action">A delegate to the code to be tested.</param>
    public static Task ThrowsAsync(Type exceptionType, Func<Task> action) => Asserter.ThrowsAsync(exceptionType, action);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type) asynchronously.
    /// </summary>
    /// <typeparam name="TException">The type of the exception expected to be thrown.</typeparam>
    /// <param name="action">A delegate to the code to be tested.</param>
    public static Task ThrowsAsync<TException>(Func<Task> action) where TException : Exception => Asserter.ThrowsAsync<TException>(action);
}
