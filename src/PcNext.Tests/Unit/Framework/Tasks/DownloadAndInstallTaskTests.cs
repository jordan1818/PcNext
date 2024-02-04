using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Framework.Tasks;

public sealed class DownloadAndInstallTaskTests
{
    private readonly ITestLogger _logger;

    public DownloadAndInstallTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Theory]
    [InlineData("")] // No Uri
    [InlineData("mock-malformed-uri")] // Malformed Uri
    [InlineData("http://mock-invalid-uri.com/without-file")] // Invalid Uri
    public async Task DownloadAndInstall_Failed_WithUri(string uri)
    {
        // -- Arrange --
        using var testData = new DownloadAndInstallTaskTestData(_logger)
            .WithInstallerUri(uri);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload(0)
            .VerifyAttemptedToInstall(0);
    }

    [Fact]
    public async Task DownloadAndInstall_Failed_OnDownload()
    {
        // -- Arrange --
        using var testData = new DownloadAndInstallTaskTestData(_logger)
            .WithValidInstallerUri()
            .WithFailedToDownload();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload()
            .VerifyAttemptedToInstall(0);
    }

    [Fact]
    public async Task DownloadAndInstall_Throws_OnDownload()
    {
        // -- Arrange --
        using var testData = new DownloadAndInstallTaskTestData(_logger)
            .WithValidInstallerUri()
            .WithDownloadThrows<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<Exception>()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload()
            .VerifyAttemptedToInstall(0);
    }

    [Fact]
    public async Task DownloadAndInstall_Throws_OnInstall()
    {
        // -- Arrange --
        using var testData = new DownloadAndInstallTaskTestData(_logger)
            .WithValidInstallerUri()
            .WithSuccessfulDownload()
            .WithInstallThrows<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<Exception>()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload()
            .VerifyAttemptedToInstall();
    }

    [Fact]
    public async Task DownloadAndInstall_Successfully()
    {
        // -- Arrange --
        using var testData = new DownloadAndInstallTaskTestData(_logger)
            .WithValidInstallerUri()
            .WithSuccessfulDownload()
            .WithSuccessfulInstall();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageEmpty()
            .IsResultsSuccessful()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload()
            .VerifyAttemptedToInstall();
    }
}
