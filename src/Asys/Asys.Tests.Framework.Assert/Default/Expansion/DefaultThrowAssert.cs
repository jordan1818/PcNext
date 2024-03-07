namespace Asys.Tests.Framework.Asserts.Default;

/// <summary>
/// The implementation of <see cref="IThrowAssert"/>
/// for default assertion sets.
/// </summary>
public sealed partial class DefaultAssert
{
    /// <inheritdoc/>
    public void Throws(Type exceptionType, Action action)
    {
        if (!exceptionType.IsSubclassOf(typeof(Exception)))
        {
            throw new AssertException($"Expected expcetion type '{exceptionType.Name}' was not of type '{nameof(Exception)}'");
        }

        Exception? exception = null;
        try
        {
            action();
        }
        catch (Exception e)
        {
            exception = e;
        }

        if (exception == null)
        {
            throw new AssertException("No exception has occurred.");
        }

        if (exception!.GetType() != exceptionType)
        {
            throw new AssertException($"Exception was not as expected. Expected: '{exceptionType.Name}' but was '{exception}'");
        }
    }

    /// <inheritdoc/>
    public void Throws<TException>(Action action) where TException : Exception => Throws(typeof(TException), action);

    /// <inheritdoc/>
    public async Task ThrowsAsync(Type exceptionType, Func<Task> action)
    {
        if (!exceptionType.IsSubclassOf(typeof(Exception)))
        {
            throw new AssertException($"Expected expcetion type '{exceptionType.Name}' was not of type '{nameof(Exception)}'");
        }

        Exception? exception = null;
        try
        {
            await action();
        }
        catch (Exception e)
        {
            exception = e;
        }

        if (exception == null)
        {
            throw new AssertException("No exception has occurred.");
        }

        if (exception!.GetType() != exceptionType)
        {
            throw new AssertException($"Exception was not as expected. Expected: '{exceptionType.Name}' but was '{exception}'");
        }
    }

    /// <inheritdoc/>
    public Task ThrowsAsync<TException>(Func<Task> action) where TException : Exception => ThrowsAsync(typeof(TException), action);
}
