namespace Asys.Tasks.Results;

/// <summary>
/// The definition of <see cref="TaskResults"/> which represents the results of a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance.
/// </summary>
public class TaskResults
{
    /// <summary>
    /// Initializes an instance of <see cref="TaskResults"/>.
    /// </summary>
    /// <param name="result">The final result of execuing the instance of <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.</param>
    /// <param name="failureMessage">The failure message should be used in conjunction with <see cref="TaskResult.Failure"/>.
    /// This message represents why the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> has failed.</param>
    /// <param name="exception">The exception caught during the execution of the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.</param>
    /// <remarks>
    /// An instance of <see cref="TaskResults"/> will be return
    /// for all <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </remarks>
    public TaskResults(TaskResult result, string? failureMessage = null, Exception? exception = null)
    {
        Result = result;
        FailureMessage = failureMessage;
        Exception = exception;
    }

    /// <summary>
    /// The final result of execuing the instance of <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </summary>
    public TaskResult Result { get; }

    /// <summary>
    /// The failure message should be used in conjunction with <see cref="TaskResult.Failure"/>.
    /// This message represents why the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> has failed.
    /// </summary>
    public string? FailureMessage { get; }

    /// <summary>
    /// The exception caught with the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Validates whether the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> was successful.
    /// Uses <see cref="Result"/> to determine this.
    /// </summary>
    public bool IsSuccessful => Result == TaskResult.Success;

    /// <summary>
    /// Method to instantiate a successful instance of <see cref="TaskResults"/> for a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>. 
    /// </summary>
    /// <returns>A successful instance of <see cref="TaskResults"/> for a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.</returns>
    public static TaskResults Success() => new (TaskResult.Success);

    /// <summary>
    /// Method to instantiate a failure instance of <see cref="TaskResults"/> for a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>. 
    /// </summary>
    /// <param name="failureMessage">The failure message in which explains why a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> has failed.</param>
    /// <param name="exception">The exception caught during the execution of the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.</param>
    /// <returns>A failure instance of <see cref="TaskResults"/> for a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.</returns>
    public static TaskResults Failure(string? failureMessage = null, Exception? exception = null) => new (TaskResult.Failure, failureMessage) { Exception = exception };
}
