using Asys.System.Diagnostics;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;

namespace Asys.Tasks.Process;

/// <summary>
/// The definition of <see cref="ProcessTask"/> based off of <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}"/>
/// for process related task executions.
/// </summary>
public class ProcessTask : ProcessTask<ProcessTaskContext, ProcessTaskResults>
{
    /// <summary>
    /// Initializes an instance of <see cref="ProcessTask"/>.
    /// </summary>
    public ProcessTask()
        : this(processFactory: null)
    {
        
    }

    /// <summary>
    /// Initializes an instance of <see cref="ProcessTask"/>.
    /// </summary>
    /// <param name="processFactory">The instance of <see cref="IProcessFactory"/> for process operations.</param>
    internal ProcessTask(IProcessFactory? processFactory)
        : base(processFactory)
    {
        
    }
}