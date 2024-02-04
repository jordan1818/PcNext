using Asys.Tasks.Contexts;
using Asys.Tasks.Results;

namespace Asys.Tasks;

/// <summary>
/// The definition of <see cref="ITask"/> which extends <see cref="ITask{T,T}"/>.
/// </summary>
public interface ITask : ITask<TaskContext, TaskResults>
{
}
