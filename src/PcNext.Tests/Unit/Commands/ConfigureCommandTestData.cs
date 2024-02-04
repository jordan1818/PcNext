using Asys.Mocks.Tasks;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tests.Framework.Asserts;
using Asys.Tests.Framework.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using PcNext.Framework;
using PcNext.Framework.Configurations;

namespace PcNext.Tests.Unit.Commands;

internal sealed class ConfigureCommandTestData : CommandTestData<ConfigureCommandTestData>
{
    public ConfigureCommandTestData(ITestLogger logger)
        : base(logger)
    {
        MockTasks = new List<Mock<ITask>>();
        MockPcNextConfigurationTaskFactory = new Mock<IPcNextConfigurationTaskFactory>();

        MockPcNextConfigurationTaskFactory.Setup(m => m.Create(It.IsAny<PcNextConfiguration>()))
            .Returns(() => MockTasks.Select(t => t.Object));
    }

    protected override string CommandArguments => $"configure{(FailOnError ? " --fail-on-error" : string.Empty)}";

    private bool FailOnError { get; set; }

    private IList<Mock<ITask>> MockTasks { get; } 

    private Mock<IPcNextConfigurationTaskFactory> MockPcNextConfigurationTaskFactory { get; }

    public ConfigureCommandTestData WithFailOnError()
    {
        FailOnError = true;
        return this;
    }

    public ConfigureCommandTestData WithSuccessfulConfigureTaskResults()
    {
        MockTasks.Add(new Mock<ITask>().WithSuccessfulResults());
        return this;
    }

    public ConfigureCommandTestData WithFailureConfigureTaskResults()
    {
        MockTasks.Add(new Mock<ITask>().WithFailureResults());
        return this;
    }

    public ConfigureCommandTestData VerifyAllConfigureTask(int expectedConfigureTaskCount, params BreakExecutionExpectation[] expectedBreakExecutionExceptations)
    {
        Assert.Equal(expectedConfigureTaskCount, MockTasks.Count);
        for (var i = 0; i < MockTasks.Count; i++)
        {
            var mockTask = MockTasks[i];

            var expectedToExecute = !expectedBreakExecutionExceptations
                .Any(e => 
                    i == e.StartIndex || 
                    (e.ToIndex != BreakExecutionExpectation.UndefinedIndex && 
                     i <= e.ToIndex && i > e.StartIndex));

            mockTask.Verify(m => m.ExecuteAsync(It.IsAny<TaskContext>()), Times.Exactly(expectedToExecute ? 1 : 0));
        }

        return this;
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.RemoveAll<IPcNextConfigurationTaskFactory>();
        services.TryAddSingleton(MockPcNextConfigurationTaskFactory.Object);
    }
}

internal struct BreakExecutionExpectation
{
    public const int UndefinedIndex = -1;

    private BreakExecutionExpectation(int startIndex, int toIndex)
    {
        StartIndex = startIndex;
        ToIndex = toIndex;
    }

    public int StartIndex { get; }

    public int ToIndex { get; }

    public static BreakExecutionExpectation At(int index) => new(index, UndefinedIndex);

    public static BreakExecutionExpectation From(int startIndex, int toIndex) => new(startIndex, toIndex);
}