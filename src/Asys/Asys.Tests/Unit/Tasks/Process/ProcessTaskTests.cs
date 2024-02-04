using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace Asys.Tests.Unit.Tasks.Process;

public sealed class ProcessTaskTests
{
    private readonly ITestLogger _logger;

    public ProcessTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcess_Failed_ToStart()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessFailedToStart();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsExceptionEmpty()
            .VerifyProcessAttemptedToStart();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcess_Throws_OnStart()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessThrowsOnStart<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsWithException<Exception>()
            .VerifyProcessAttemptedToStart();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcess_Throws_OnWaitingForExit()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessThrowsOnWaitingForExit<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsWithException<Exception>()
            .VerifyProcessAttemptedToWaitForExit();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcessThrows_TaskCanceledException_OnWaitingForExit()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessThrowsOnWaitingForExit<TaskCanceledException>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsWithException<TaskCanceledException>()
            .VerifyProcessAttemptedToWaitForExit();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcessThrows_TaskCanceledException_WithInnerException_AsTimeoutException_OnWaitingForExit()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessThrowsOnWaitingForExit(
            new TaskCanceledException(
                nameof(ExecuteAsync_Failure_WithProcessThrows_TaskCanceledException_WithInnerException_AsTimeoutException_OnWaitingForExit), 
                new TimeoutException()));

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsWithException<TimeoutException>()
            .VerifyProcessAttemptedToWaitForExit();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcess_Still_Alive()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessThrowsOnWaitingForExit<Exception>()
            .WithProcessStillAlive();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsWithException<Exception>()
            .VerifyProcessAttemptedToWaitForExit()
            .VerifyProcessAttemptedToKill();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_WithProcess_NotExpected_ExitCode()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessExitCode(1);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsExceptionEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Success_WithSuccessfulProcess()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .IsResultsFailureMessageEmpty()
            .IsResultsExceptionEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Success_WithProcess_Still_Alive()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithSuccessfulProcess()
            .WithProcessStillAlive();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .IsResultsFailureMessageEmpty()
            .IsResultsExceptionEmpty()
            .VerifyProcessAttemptedToKill();
    }
}
