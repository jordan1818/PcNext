namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="IThrowAssert"/>
/// for general throw assertion sets.
/// </summary>
public interface IThrowAssert
{
    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <param name="exceptionType">The type of the exception expected to be thrown.</param>
    /// <param name="action">A delegate to the code to be tested.</param>
    void Throws(Type exceptionType, Action action);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TException">The type of the exception expected to be thrown.</typeparam>
    /// <param name="action">A delegate to the code to be tested.</param>
    void Throws<TException>(Action action) where TException : Exception;

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type) asynchronously.
    /// </summary>
    /// <param name="exceptionType">The type of the exception expected to be thrown.</param>
    /// <param name="action">A delegate to the code to be tested.</param>
    Task ThrowsAsync(Type exceptionType, Func<Task> action);

    /// <summary>
    /// Verifies that the exact exception is thrown (and not a derived exception type) asynchronously.
    /// </summary>
    /// <typeparam name="TException">The type of the exception expected to be thrown.</typeparam>
    /// <param name="action">A delegate to the code to be tested.</param>
    Task ThrowsAsync<TException>(Func<Task> action) where TException : Exception;
}
