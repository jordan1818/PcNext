using Asys.System.Diagnostics;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;

namespace Asys.Tasks.Process;

/// <summary>
/// The definition of <see cref="IProcessTaskFactory"/> 
/// to create an instance of <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/> 
/// for process related task executions.
/// </summary>
public interface IProcessTaskFactory
{
    /// <summary>
    /// Creates an instance of <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/>.
    /// </summary>
    /// <typeparam name="TProcessTaskContext">The process task context which must be derived from <see cref="ProcessTaskContext"/>.</typeparam>
    /// <typeparam name="TProcessTaskResults">The process task results which must be derived from <see cref="ProcessTaskResults"/>.</typeparam>
    /// <param name="processFactory">The instance of <see cref="IProcessFactory"/> for process operations.</param>
    /// <returns>An instance of <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/>.</returns>
    ProcessTask<TProcessTaskContext, TProcessTaskResults> Create<TProcessTaskContext, TProcessTaskResults>()
        where TProcessTaskContext : ProcessTaskContext
        where TProcessTaskResults : ProcessTaskResults;

    /// <summary>
    /// Creates an instance of <see cref="ProcessTask"/>.
    /// </summary>
    /// <param name="processFactory">The instance of <see cref="IProcessFactory"/> for process operations.</param>
    /// <returns>An instance of <see cref="ProcessTask"/>.</returns>
    ProcessTask Create();
}
