using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Moq;

namespace Asys.Mocks.Tasks;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{ITask}"/> instance.
/// </summary>
public static class MockTaskExtensions
{
    /// <summary>
    /// Setups a result on a mocked <see cref="ITask{TTaskContext, TTaskResults}.ExecuteAsync(TTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockTask">The instance of the <see cref="Mock{ITask}"/> to configure.</param>
    /// <param name="taskResults">The instance of <see cref="TaskResults"/> to be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITask}"/> to allow for chainning.</returns>
    public static Mock<ITask> WithResults(this Mock<ITask> mockTask, TaskResults taskResults)
    {
        mockTask.Setup(m => m.ExecuteAsync(It.IsAny<TaskContext>()))
            .ReturnsAsync(taskResults);
        return mockTask;
    }

    /// <summary>
    /// Setups a successful result on a mocked <see cref="ITask{TTaskContext, TTaskResults}.ExecuteAsync(TTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockTask">The instance of the <see cref="Mock{ITask}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITask}"/> to allow for chainning.</returns>
    public static Mock<ITask> WithSuccessfulResults(this Mock<ITask> mockTask)
        => mockTask.WithResults(TaskResults.Success());

    /// <summary>
    /// Setups a failure result on <see cref="ITask{TTaskContext, TTaskResults}.ExecuteAsync(TTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockTask">The instance of the <see cref="Mock{ITask}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITask}"/> to allow for chainning.</returns>
    public static Mock<ITask> WithFailureResults(this Mock<ITask> mockTask, string? failureMessage = null, Exception? exception = null)
        => mockTask.WithResults(TaskResults.Failure(failureMessage, exception));

    /// <summary>
    /// Setups a throw on a mocked <see cref="ITask{TTaskContext, TTaskResults}.ExecuteAsync(TTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockTask">The instance of the <see cref="Mock{ITask}"/> to configure.</param>
    /// <param name="exception">The <see cref="Exception"/> instance that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITask}"/> to allow for chainning.</returns>
    public static Mock<ITask> Throws(this Mock<ITask> mockTask, Exception exception)
    {
        mockTask.Setup(m => m.ExecuteAsync(It.IsAny<TaskContext>()))
                .Throws(exception);
        return mockTask;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="ITask{TTaskContext, TTaskResults}.ExecuteAsync(TTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockTask">The instance of the <see cref="Mock{ITask}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITask}"/> to allow for chainning.</returns>
    public static Mock<ITask> Throws<TException>(this Mock<ITask> mockTask)
        where TException : Exception, new()
    => mockTask.Throws(new TException());
}
