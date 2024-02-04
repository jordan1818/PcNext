using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Framework.Tasks;

public sealed class OnStartUpTaskTests
{
    private readonly ITestLogger _logger;

    public OnStartUpTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task ExecuteAsync_Failure_NotOnWindows()
    {
        // -- Arrange --
        using var testData = new OnStartUpTaskTestData(_logger)
            .WithOsIsNotWindows();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyStartupTaskWasRegistered(0);
    }

    [Fact]
    public async Task ExecuteAsync_Failure_AccountCouldNotBeRetrieved()
    {
        // -- Arrange --
        using var testData = new OnStartUpTaskTestData(_logger)
            .WithNotFoundAccount();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyStartupTaskWasRegistered(0);
    }

    [Fact]
    public async Task ExecuteAsync_Failure_InvalidFolder()
    {
        // -- Arrange --
        using var testData = new OnStartUpTaskTestData(_logger)
            .WithAInvalidFolder();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyStartupTaskWasRegistered(0);
    }

    [Fact]
    public async Task ExecuteAsync_Failure_InvalidCommand()
    {
        // -- Arrange --
        using var testData = new OnStartUpTaskTestData(_logger)
            .WithAInvalidCommand();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyStartupTaskWasRegistered(0);
    }

    [Fact]
    public async Task ExecuteAsync_Successful()
    {
        // -- Arrange --
        using var testData = new OnStartUpTaskTestData(_logger);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageEmpty()
            .IsResultsSuccessful()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyStartupTaskWasRegistered();
    }
}
