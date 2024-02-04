namespace Asys.Tasks.Results;

/// <summary>=
/// The definition of <see cref="TaskResult"/> of a <see cref"ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance.
/// Used within and returned with <see cref="TaskResults"/> after the completion of a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
/// </summary>
public enum TaskResult
{
    /// <summary>
    /// This result represents 
    /// that a <see cref="ITask"/> 
    /// or <see cref="ITask{TTaskContext, TTaskContext}"/>
    /// was successful.
    /// This is the default result.
    /// </summary>
    Success,

    /// <summary>
    /// This result represents 
    /// that a <see cref="ITask"/>
    /// or <see cref="ITask{TTaskContext, TTaskContext}"/>
    /// has failed.
    /// </summary>
    Failure,
}
