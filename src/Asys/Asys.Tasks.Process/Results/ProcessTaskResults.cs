using Asys.Tasks.Results;

namespace Asys.Tasks.Process.Results;

/// <summary>
/// The definition of <see cref="ProcessTaskResults"/> which extends <see cref="TaskResults"/> for the results of a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/> instance.
/// </summary>
public class ProcessTaskResults : TaskResults
{
    /// <summary>
    /// Initializes an instance of <see cref="ProcessTaskResults"/>.
    /// </summary>
    /// <param name="result">The final result of execuing the instance of <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/>.</param>
    /// <param name="failureMessage">The failure message should be used in conjunction with <see cref="TaskResult.Failure"/>.
    /// This message represents why the <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/> has failed.</param>
    /// <param name="exception">The exception caught during the execution of the <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/>.</param>
    public ProcessTaskResults(TaskResult result, string? failureMessage = null, Exception? exception = null)
        : base(result, failureMessage, exception)
    {
    }

    /// <summary>
    /// The returned standard output of the <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/> instance.
    /// </summary>
    public string? StandardOutput { get; set; }

    /// <summary>
    /// The returned standard error of the <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/> instance.
    /// </summary>
    public string? StandardError { get; set; }

    /// <summary>
    /// Method to instantiate a failure instance of <see cref="ProcessTaskResults"/> for a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/>.
    /// </summary>
    /// <param name="standardOutput">The standard output of from the <see cref="ProcessTask"/>.</param>
    /// <param name="standardError">The standard error of from the <see cref="ProcessTask"/>.</param>
    /// <returns>A successful instance of <see cref="ProcessTaskResults"/> for a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/>.</returns>
    public static ProcessTaskResults Success(string? standardOutput = null, string? standardError = null) =>
        new (TaskResult.Success)
        {
            StandardOutput = standardOutput,
            StandardError = standardError,
        };

    /// <summary>
    /// Method to instantiate a failure instance of <see cref="ProcessTaskResults"/> for a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/>. 
    /// </summary>
    /// <param name="failureMessage">The failure message in which explains why a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/> has failed.</param>
    /// <param name="exception">The exception caught during the execution of the <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/></param>
    /// <returns>A failure instance of <see cref="ProcessTaskResults"/> for a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/>.</returns>
    public static new ProcessTaskResults Failure(string? failureMessage = null, Exception? exception = null) => new(TaskResult.Failure, failureMessage) { Exception = exception };
}
