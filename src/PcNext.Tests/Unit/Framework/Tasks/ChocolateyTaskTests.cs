using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using PcNext.Tests.Unit.Commands;
using Xunit;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Framework.Tasks;

public sealed class ChocolateyTaskTests
{
    private readonly ITestLogger _logger;

    public ChocolateyTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task InstallPackage_Successfully()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulInstall();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .VerifyAttemptToInstallPackage();
    }

    [Fact]
    public async Task InstallPackage_Successfully_WithBeforeTasks()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulInstall()
            .WithSuccessfulBeforeTask()
            .WithSuccessfulBeforeTask()
            .WithSuccessfulBeforeTask();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .VerifyAllBeforeTasks(3)
            .VerifyAttemptToInstallPackage();
    }

    [Fact]
    public async Task InstallPackage_Successfully_WithAfterTasks()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulInstall()
            .WithSuccessfulAfterTask()
            .WithSuccessfulAfterTask()
            .WithSuccessfulAfterTask();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .VerifyAttemptToInstallPackage()
            .VerifyAllAfterTasks(3);
    }

    [Fact]
    public async Task InstallPackage_Successfully_WithBeforeAndAfterTasks()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulInstall()
            .WithSuccessfulBeforeTask()
            .WithSuccessfulBeforeTask()
            .WithSuccessfulBeforeTask()
            .WithSuccessfulAfterTask()
            .WithSuccessfulAfterTask()
            .WithSuccessfulAfterTask();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsSuccessful()
            .VerifyAllBeforeTasks(3)
            .VerifyAttemptToInstallPackage()
            .VerifyAllAfterTasks(3);
    }

    [Fact]
    public async Task InstallPackage_Failed()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithFailedToInstall();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .VerifyAttemptToInstallPackage();
    }

    [Fact]
    public async Task InstallPackage_Failed_With_NoName()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .VerifyAttemptToInstallPackage(0);
    }

    [Fact]
    public async Task InstallPackage_Failed_OnBeforeTasks()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulBeforeTask()
            .WithFailedBeforeTask()
            .WithSuccessfulBeforeTask();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .VerifyAllBeforeTasks(3, BreakExecutionExpectation.At(2))
            .VerifyAttemptToInstallPackage(0);
    }

    [Fact]
    public async Task InstallPackage_Failed_OnAfterTasks()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulInstall()
            .WithSuccessfulAfterTask()
            .WithFailedAfterTask()
            .WithSuccessfulAfterTask();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .VerifyAttemptToInstallPackage()
            .VerifyAllAfterTasks(3, BreakExecutionExpectation.At(2));
    }

    [Fact]
    public async Task InstallPackage_Failed_OnBeforeAndAfterTasks()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithSuccessfulInstall()
            .WithSuccessfulBeforeTask()
            .WithFailedBeforeTask()
            .WithSuccessfulBeforeTask()
            .WithSuccessfulAfterTask()
            .WithFailedAfterTask()
            .WithSuccessfulAfterTask();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .VerifyAllBeforeTasks(3, BreakExecutionExpectation.At(2))
            .VerifyAttemptToInstallPackage(0)
            .VerifyAllAfterTasks(3, BreakExecutionExpectation.From(0, 2));
    }

    [Fact]
    public async Task InstallPackage_Throws_OnInstall()
    {
        // -- Arrange --
        using var testData = new ChocolateyTaskTestData(_logger)
            .WithPackage("mock-package")
            .WithInstallThrows<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailure()
            .VerifyAttemptToInstallPackage();
    }
}
