namespace Asys.Tasks;

/// <summary>
/// The definition of <see cref="TaskResultsException"/> base off of <see cref="Exception"/>.
/// </summary>
public class TaskResultsException : Exception
{
    /// <summary>
    /// Initializes an instance of <see cref="TaskResultsException"/>.
    /// </summary>
    /// <param name="message">The message for the <see cref="TaskResultsException"/>.</param>
    public TaskResultsException(string? message)
    : this(message, innerException: null)
    {

    }

    /// <summary>
    /// Initializes an instance of <see cref="TaskResultsException"/>.
    /// </summary>
    /// <param name="message">The message for the <see cref="TaskResultsException"/>.</param>
    /// <param name="innerException">The inner exception that occurred along side with <see cref="TaskResultsException"/>.</param>
    public TaskResultsException(string? message, Exception? innerException)
        : base(message, innerException)
    {

    }
}
