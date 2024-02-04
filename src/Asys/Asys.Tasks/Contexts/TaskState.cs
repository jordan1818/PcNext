namespace Asys.Tasks.Contexts;

/// <summary>
/// The definition of <see cref="TaskState"/> for a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance.
/// Used within <see cref="TaskContext"/>.
/// </summary>
public enum TaskState
{
    /// <summary>
    /// This state is the default state 
    /// if the <see cref="ITask"/ > instance does
    /// not update it's state or just started.
    /// </summary>
    Unknown,

    /// <summary>
    /// This state of a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance is
    /// used when preparing the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> 
    /// instance before <see cref="Running"/>.
    /// </summary>
    Preparing,

    /// <summary>
    /// This state of a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance is
    /// used when running the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </summary>
    Running,

    /// <summary>
    /// This state of a <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance is
    /// used after the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// This does not imply that the instance of <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>
    /// has successed or failed, only that it has finished.
    /// </summary>
    Completed,
}
