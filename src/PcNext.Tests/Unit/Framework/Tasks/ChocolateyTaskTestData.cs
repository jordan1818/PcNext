using Asys.Mocks.Tasks;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Asys.Tests.Framework.Asserts;
using Asys.Tests.Framework.Data.Tasks;
using Asys.Tests.Framework.Logger;
using ChocolateySharp;
using ChocolateySharp.Mocks;
using ChocolateySharp.Options;
using Moq;
using PcNext.Framework;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;
using PcNext.Tests.Unit.Commands;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal sealed class ChocolateyTaskTestData : TaskTestData<ChocolateyTask, TaskContext, TaskResults, ChocolateyTaskTestData>
{
    public ChocolateyTaskTestData(ITestLogger logger)
    {
        Context = new TaskContext(logger);
        ChocolateyConfiguration = new ChocolateyConfiguration();
        MockChocolateyClient = new Mock<IChocolateyClient>();
        MockBeforeTasks = new List<Mock<ITask>>();
        MockAfterTasks = new List<Mock<ITask>>();
    }

    protected override TaskContext Context { get; set; }

    private ChocolateyConfiguration ChocolateyConfiguration { get; }

    private Mock<IChocolateyClient> MockChocolateyClient { get; }

    private IList<Mock<ITask>> MockBeforeTasks { get; }

    private IList<Mock<ITask>> MockAfterTasks { get; }

    protected override ChocolateyTask CreateTask() 
    {
        var mockTaskConfigurationTaskFactory = new Mock<ITaskConfigurationTaskFactory>();
        var mockTaskConfigurationTaskFactorySequence = mockTaskConfigurationTaskFactory.SetupSequence(m => m.Create(It.IsAny<TaskConfiguration>()));

        foreach (var mockTask in MockBeforeTasks.Union(MockAfterTasks)) 
        {
            mockTaskConfigurationTaskFactorySequence.Returns(() => mockTask.Object);
        }

        return new(ChocolateyConfiguration, MockChocolateyClient.Object, mockTaskConfigurationTaskFactory.Object);
    }

    public ChocolateyTaskTestData WithPackage(string name)
    {
        ChocolateyConfiguration.Name = name;
        return this;
    }

    public ChocolateyTaskTestData WithSuccessfulBeforeTask()
    {
        ChocolateyConfiguration.BeforeTasks.Add(new TaskConfiguration());
        MockBeforeTasks.Add(new Mock<ITask>().WithSuccessfulResults());
        return this;
    }

    public ChocolateyTaskTestData WithFailedBeforeTask()
    {
        ChocolateyConfiguration.BeforeTasks.Add(new TaskConfiguration());
        MockBeforeTasks.Add(new Mock<ITask>().WithFailureResults());
        return this;
    }

    public ChocolateyTaskTestData WithSuccessfulAfterTask()
    {
        ChocolateyConfiguration.AfterTasks.Add(new TaskConfiguration());
        MockAfterTasks.Add(new Mock<ITask>().WithSuccessfulResults());
        return this;
    }

    public ChocolateyTaskTestData WithFailedAfterTask()
    {
        ChocolateyConfiguration.AfterTasks.Add(new TaskConfiguration());
        MockAfterTasks.Add(new Mock<ITask>().WithFailureResults());
        return this;
    }

    public ChocolateyTaskTestData WithSuccessfulInstall()
    {
        MockChocolateyClient.WithSuccessfullyPackageInstall();
        return this;
    }

    public ChocolateyTaskTestData WithFailedToInstall()
    {
        MockChocolateyClient.WithFailedToInstallPackage();
        return this;
    }

    public ChocolateyTaskTestData WithInstallThrows(Exception exception)
    {
        MockChocolateyClient.WithPackageInstallThrows(exception);
        return this;
    }

    public ChocolateyTaskTestData WithInstallThrows<TException>()
        where TException : Exception, new()
    {
        MockChocolateyClient.WithPackageInstallThrows<TException>();
        return this;
    }

    public ChocolateyTaskTestData VerifyAttemptToInstallPackage(int expectedInstallCount = 1)
    {
        MockChocolateyClient.Verify(m => m.InstallPackageAsync(It.IsAny<string>(), It.IsAny<ChocolateyPackageInstallOptions>(), It.IsAny<CancellationToken>()), Times.Exactly(expectedInstallCount));
        return this;
    }

    public ChocolateyTaskTestData VerifyAllBeforeTasks(int expectedBeforeTaskCount, params BreakExecutionExpectation[] expectedBreakExecutionExceptations)
    {
        Assert.Equal(expectedBeforeTaskCount, MockBeforeTasks.Count);
        for (var i = 0; i < MockBeforeTasks.Count; i++)
        {
            var mockTask = MockBeforeTasks[i];

            var expectedToExecute = !expectedBreakExecutionExceptations
                .Any(e =>
                    i == e.StartIndex ||
                    (e.ToIndex != BreakExecutionExpectation.UndefinedIndex &&
                     i <= e.ToIndex && i > e.StartIndex));

            mockTask.Verify(m => m.ExecuteAsync(It.IsAny<TaskContext>()), Times.Exactly(expectedToExecute ? 1 : 0));
        }

        return this;
    }

    public ChocolateyTaskTestData VerifyAllAfterTasks(int expectedAfterTaskCount, params BreakExecutionExpectation[] expectedBreakExecutionExceptations)
    {
        Assert.Equal(expectedAfterTaskCount, MockAfterTasks.Count);
        for (var i = 0; i < MockAfterTasks.Count; i++)
        {
            var mockTask = MockAfterTasks[i];

            var expectedToExecute = !expectedBreakExecutionExceptations
                .Any(e =>
                    i == e.StartIndex ||
                    (e.ToIndex != BreakExecutionExpectation.UndefinedIndex &&
                     i <= e.ToIndex && i > e.StartIndex));

            mockTask.Verify(m => m.ExecuteAsync(It.IsAny<TaskContext>()), Times.Exactly(expectedToExecute ? 1 : 0));
        }

        return this;
    }
}
