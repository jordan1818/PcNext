using Asys.System.Diagnostics;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;

namespace Asys.Tasks.Process;

/// <summary>
/// The implementation of <see cref="IProcessTaskFactory"/>
/// within <see cref="ProcessTaskFactory"/>.
/// </summary>
public sealed class ProcessTaskFactory : IProcessTaskFactory
{
    /// <inheritdoc/>
    public ProcessTask<TProcessTaskContext, TProcessTaskResults> Create<TProcessTaskContext, TProcessTaskResults>()
        where TProcessTaskContext : ProcessTaskContext
        where TProcessTaskResults : ProcessTaskResults
    => new ();

    /// <inheritdoc/>
    public ProcessTask Create() => new ();
}
