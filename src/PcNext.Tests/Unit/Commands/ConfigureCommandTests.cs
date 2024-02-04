using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Commands;

public sealed class ConfigureCommandTests
{
    private readonly ITestLogger _logger;

    public ConfigureCommandTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task InvokeAsync_NoConfigureTask_Successfully()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIsSuccess();
    }

    [Fact]
    public async Task InvokeAsync_SingleConfigureTask_Successfully()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithSuccessfulConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIsSuccess()
            .VerifyAllConfigureTask(1);
    }

    [Fact]
    public async Task InvokeAsync_SingleConfigureTask_AndFailTask_ButSucceed()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithFailureConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIsSuccess()
            .VerifyAllConfigureTask(1);
    }

    [Fact]
    public async Task InvokeAsync_SingleConfigureTask_AndFailTask_ButFail()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithFailOnError()
            .WithFailureConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIs(1)
            .VerifyAllConfigureTask(1);
    }

    [Fact]
    public async Task InvokeAsync_MultipleConfigureTasks_Successfully()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIsSuccess()
            .VerifyAllConfigureTask(5);
    }

    [Fact]
    public async Task InvokeAsync_MultipleConfigureTask_AndOneTaskFails_ButSucceed()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithFailureConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIsSuccess()
            .VerifyAllConfigureTask(5);
    }

    [Fact]
    public async Task InvokeAsync_MultipleConfigureTask_AndMultipleTasksFails_ButSucceed()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithFailureConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithFailureConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithFailureConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIsSuccess()
            .VerifyAllConfigureTask(5);
    }

    [Fact]
    public async Task InvokeAsync_MultipleConfigureTask_AndOneTaskFails_ButFail()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithFailOnError()
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithFailureConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIs(1)
            .VerifyAllConfigureTask(5, BreakExecutionExpectation.From(3, 4));
    }

    [Fact]
    public async Task InvokeAsync_MultipleConfigureTask_AndMultipleTasksFails_ButFail()
    {
        // -- Arrange --
        using var testData = new ConfigureCommandTestData(_logger)
            .WithFailOnError()
            .WithFailureConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithFailureConfigureTaskResults()
            .WithSuccessfulConfigureTaskResults()
            .WithFailureConfigureTaskResults();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .EnsureExitCodeIs(1)
            .VerifyAllConfigureTask(5, BreakExecutionExpectation.From(1, 4));
    }
}
