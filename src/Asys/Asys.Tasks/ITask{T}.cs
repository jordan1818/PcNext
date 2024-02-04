using Asys.Tasks.Contexts;
using Asys.Tasks.Results;

namespace Asys.Tasks;

/// <summary>
/// The definition of <see cref="ITask{TTaskContext, TTaskContext}"/>.
/// </summary>
/// <typeparam name="TTaskContext">The task context which must be derived from <see cref="TaskContext"/>.</typeparam>
/// <typeparam name="TTaskResults">The task results which must be derived from <see cref="TaskResults"/>.</typeparam>
public interface ITask<TTaskContext, TTaskResults>
    where TTaskContext : TaskContext
    where TTaskResults : TaskResults
{
    /// <summary>
    /// Executes asynchronously the <see cref="ITask{TTaskContext, TTaskContext}"/> operations with context passed and returns the results.
    /// </summary>
    /// <param name="context">The instance of <see cref="TTaskContext"/> for the execution of this task.</param>
    /// <returns>Returns an instance of <see cref="TTaskResults"/> of the operations results of the task.</returns>
    Task<TTaskResults> ExecuteAsync(TTaskContext context);
}
