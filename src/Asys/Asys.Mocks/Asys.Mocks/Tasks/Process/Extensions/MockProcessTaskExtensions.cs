using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;
using Moq;

namespace Asys.Mocks.Tasks.Process;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{ProcessTask}"/> instance.
/// </summary>
public static class MockProcessTaskExtensions
{
    /// <summary>
    /// Setups a result on a mocked <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}.ExecuteAsync(TProcessTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockProcessTask">The instance of the <see cref="Mock{ProcessTask}"/> to configure.</param>
    /// <param name="processTaskResults">The instance of <see cref="ProcessTaskResults"/> to be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ProcessTask}"/> to allow for chainning.</returns>
    public static Mock<ProcessTask> WithResults(this Mock<ProcessTask> mockProcessTask, ProcessTaskResults processTaskResults)
    {
        mockProcessTask.Setup(m => m.ExecuteAsync(It.IsAny<ProcessTaskContext>()))
            .ReturnsAsync(processTaskResults);
        return mockProcessTask;
    }

    /// <summary>
    /// Setups a successful result on a  mocked <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}.ExecuteAsync(TProcessTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockProcessTask">The instance of the <see cref="Mock{ProcessTask}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ProcessTask}"/> to allow for chainning.</returns>
    public static Mock<ProcessTask> WithSuccessfulResults(this Mock<ProcessTask> mockProcessTask)
        => mockProcessTask.WithResults(ProcessTaskResults.Success());

    /// <summary>
    /// Setups a failure result on a mocked <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}.ExecuteAsync(TProcessTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockProcessTask">The instance of the <see cref="Mock{ProcessTask}"/> to configure.</param>
    /// <param name="failureMessage">The failure message in which explains why a <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/> has failed.</param>
    /// <param name="exception">The exception caught during the execution of the <see cref="ProcessTask"/> or <see cref="ProcessTask{TProcessTaskContext, TProcessTaskContext}"/></param>
    /// <returns>The confgiured instane of <see cref="Mock{ProcessTask}"/> to allow for chainning.</returns>
    public static Mock<ProcessTask> WithFailureResults(this Mock<ProcessTask> mockProcessTask, string? failureMessage = null, Exception? exception = null)
        => mockProcessTask.WithResults(ProcessTaskResults.Failure(failureMessage, exception));

    /// <summary>
    /// Setups a throw on a mocked <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}.ExecuteAsync(TProcessTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockProcessTask">The instance of the <see cref="Mock{ProcessTask{TProcessTaskContext, TProcessTaskResults}}"/> to configure.</param>
    /// <param name="exception">The <see cref="Exception"/> instance that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ProcessTask{TProcessTaskContext, TProcessTaskResults}}"/> to allow for chainning.</returns>
    public static Mock<ProcessTask> Throws(this Mock<ProcessTask> mockProcessTask, Exception exception)
    {
        mockProcessTask.Setup(m => m.ExecuteAsync(It.IsAny<ProcessTaskContext>()))
                .Throws(exception);
        return mockProcessTask;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="ProcessTask{TProcessTaskContext, TProcessTaskResults}.ExecuteAsync(TProcessTaskContext)"/> instance.
    /// </summary>
    /// <param name="mockProcessTask">The instance of the <see cref="Mock{ProcessTask{TProcessTaskContext, TProcessTaskResults}}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ProcessTask{TProcessTaskContext, TProcessTaskResults}}"/> to allow for chainning.</returns>
    public static Mock<ProcessTask> Throws<TException>(this Mock<ProcessTask> mockProcessTask)
        where TException : Exception, new()
    => mockProcessTask.Throws(new TException());
}
