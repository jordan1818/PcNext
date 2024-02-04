using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Asys.Tests.Framework.Asserts;

namespace Asys.Tests.Framework.Data.Tasks;

/// <summary>
/// The definition of <see cref="ITask{TTaskContext, TTaskResults}"/> test data.
/// </summary>
/// <typeparam name="TTask">The type of <see cref="ITask{TTaskContext, TTaskResults}"/> to be tested on.</typeparam>
/// <typeparam name="TTaskContext">The type of <see cref="TaskContext"/> for the instance of <see cref="ITask{TTaskContext, TTaskResults}"/> to consume.</typeparam>
/// <typeparam name="TTaskResults">The type of <see cref="TaskResults"/> in which <see cref="ITask{TTaskContext, TTaskResults}"/> returns.</typeparam>
/// <typeparam name="TTaskTestData">The object in which is based off of <see cref="TaskTestData{TTask, TTaskContext, TTaskResults, TTaskTestData}"/>.</typeparam>
public abstract class TaskTestData<TTask, TTaskContext, TTaskResults, TTaskTestData> : ITestData
    where TTask : ITask<TTaskContext, TTaskResults>
    where TTaskContext : TaskContext
    where TTaskResults : TaskResults
    where TTaskTestData : TaskTestData<TTask, TTaskContext, TTaskResults, TTaskTestData>
{
    private Exception? Exception { get; set; }

    /// <summary>
    /// The object of <see cref="TTaskContext"/> for <see cref="TTask"/> to consume.
    /// </summary>
    protected abstract TTaskContext Context { get; set; }

    /// <summary>
    /// The returned results of <see cref="TTask"/>.
    /// </summary>
    /// <remarks>
    /// Only is instanciated when <see cref="ActAsync(ActOptions?)"/>
    /// is executed.
    /// </remarks>
    protected TTaskResults? Results { get; private set; }

    /// <summary>
    /// The method to instanciate the instance of <see cref="TTask"/>.
    /// </summary>
    /// <returns>An instance of <see cref="TTask"/>.</returns>
    protected abstract TTask CreateTask();

    /// <inheritdoc/>
    public Task ActAsync(ActOptions? actOptions = null)
    {
        return Task.Run(async () =>
        {
            try
            {
                var task = CreateTask();
                Results = await task.ExecuteAsync(Context);
            }
            catch (Exception e)
            {
                Exception = e;
            }
        });
    }

    /// <summary>
    /// Validates <see cref="TTaskResults.Result"/> is one of <see cref="TaskResult"/>.
    /// </summary>
    /// <param name="taskResult">The expected <see cref="TaskResult"/> to validate against.</param>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResults(TaskResult taskResult)
    {
        Assert.NotNull(Results);
        Assert.Equal(taskResult, Results.Result);
        return (TTaskTestData)this;
    }

    /// <summary>
    /// Validates <see cref="TTaskResults.Result"/> is of result <see cref="TaskResult.Failure"/>.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsFailure() => IsResults(TaskResult.Failure);

    /// <summary>
    /// Validates <see cref="TTaskResults.Result"/> is of result <see cref="TaskResult.Success"/>.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsSuccessful() => IsResults(TaskResult.Success);

    /// <summary>
    /// Validates <see cref="TTaskResults.Exception"/> is of a <see cref="Exception"/> instance.
    /// </summary>
    /// <param name="exception">The expected <see cref="Exception"/> from <see cref="TTaskResults.Exception"/>.</param>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsException(Exception? exception)
    {
        Assert.NotNull(Results);
        if (exception is not null)
        {
            Assert.IsType(exception.GetType(), Results.Exception);
        }
        else
        {
            Assert.Null(Results.Exception);
        }

        return (TTaskTestData)this;
    }

    /// <summary>
    /// Validates if <see cref="TTask"/> threw a <see cref="Exception"/>.
    /// </summary>
    /// <param name="exception">The expected <see cref="Exception"/> from <see cref="TTask"/> to throw.</param>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsTaskException(Exception? exception)
    {
        if (exception is not null)
        {
            Assert.IsType(exception.GetType(), Exception);
        }
        else
        {
            Assert.Null(Exception);
        }

        return (TTaskTestData)this;
    }

    /// <summary>
    /// Validates <see cref="TTaskResults.FailureMessage"/> is of a expected failure message.
    /// </summary>
    /// <param name="failureMessage">The expected failure message from <see cref="TTaskResults.FailureMessage"/>.</param>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsFailureMessage(string? failureMessage)
    {
        Assert.NotNull(Results);
        Assert.Equal(failureMessage, Results.FailureMessage);
        return (TTaskTestData)this;
    }

    /// <summary>
    /// Validates <see cref="TTaskResults.FailureMessage"/> to be not empty.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsFailureMessageNotEmpty()
    {
        Assert.NotNull(Results);
        Assert.False(string.IsNullOrWhiteSpace(Results.FailureMessage));
        return (TTaskTestData)this;
    }

    /// <summary>
    /// Validates <see cref="TTaskResults.Exception"/> to be not empty.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsExceptionNotEmpty()
    {
        Assert.NotNull(Results);
        Assert.NotNull(Results.Exception);
        return (TTaskTestData)this;
    }

    /// <summary>
    /// Validates <see cref="TTaskResults.FailureMessage"/> to be empty.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsFailureMessageEmpty() => IsResultsFailureMessage(null);

    /// <summary>
    /// Validates <see cref="TTaskResults.Exception"/> to be empty.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsExceptionEmpty() => IsResultsException(null);

    /// <summary>
    /// Validates <see cref="TTask"/> <see cref="Exception"/> to be empty.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsTaskExceptionEmpty() => IsTaskException(null);

    /// <summary>
    /// Validates <see cref="TTaskResults.Exception"/> to be of a certain type.
    /// </summary>
    /// <typeparam name="TException">The type of <see cref="Exception"/> to be expected.</typeparam>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsResultsWithException<TException>()
        where TException : Exception, new()
        => IsResultsException(new TException());

    /// <summary>
    /// Validates <see cref="TTask"/> <see cref="Exception"/> to be of a certain type.
    /// </summary>
    /// <returns>The instance of <see cref="TTaskTestData"/> for chaining purposes.</returns>
    public TTaskTestData IsTaskWithException<TException>()
        where TException : Exception, new()
        => IsTaskException(new TException());

    /// <summary>
    /// Dispose of any resourses for <see cref="TaskTestData{TTask, TTaskContext, TTaskResults, TTaskTestData}"/>.
    /// </summary>
    public virtual void Dispose() { }
}
