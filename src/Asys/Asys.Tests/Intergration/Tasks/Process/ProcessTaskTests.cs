using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace Asys.Tests.Intergration.Tasks.Process;

public sealed class ProcessTaskTests
{
    private readonly ITestLogger _logger;

    public ProcessTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task ExecuteAsync_Success_AsPowerShell()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithPowerShellContext("Write-Host \"hello world\"");

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .IsResultsFailureMessageEmpty()
            .IsResultsExceptionEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Success_AsCmd()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithCmdContext("echo \"hello world\"");

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .IsResultsFailureMessageEmpty()
            .IsResultsExceptionEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_AsPowerShell()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithPowerShellContext("exit 1");

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsExceptionEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_AsCmd()
    {
        // -- Arrange --
        using var testData = new ProcessTaskTestData(_logger)
            .WithCmdContext("exit 1");

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .IsResultsFailureMessageNotEmpty()
            .IsResultsExceptionEmpty();
    }
}
