using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Framework.Tasks;

public sealed class DownloadTaskTests
{
    private readonly ITestLogger _logger;

    public DownloadTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Theory]
    [InlineData("")] // No Uri
    [InlineData("mock-malformed-uri")] // Malformed Uri
    [InlineData("http://mock-invalid-uri.com/without-file")] // Invalid Uri
    public async Task Download_Failed_WitInvalidhUri(string uri)
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithUri(uri);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload(0);
    }

    [Theory]
    [InlineData("")] // Empty destination
    [InlineData("invalid-destination")] // Invalid destination
    public async Task Download_Failed_WithInvalidDestination(string destination)
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithDestination(destination);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload(0);
    }

    [Fact]
    public async Task Download_Failed_FileAlreadyExists()
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithADestination()
            .WithFileAlreadyExists();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedFileExists()
            .VerifyAttemptedToDownload(0);
    }

    [Fact]
    public async Task Download_Failed_OverwriteFile()
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithADestination()
            .WithOverwriteFile()
            .WithFileAlreadyExists()
            .WithDeleteFileThrows<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<Exception>()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedFileExists()
            .VerifyAttemptedFileDeletion()
            .VerifyAttemptedToDownload(0);
    }

    [Fact]
    public async Task Download_Failed_CreateDestination()
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithADestination()
            .WithDestinationDoesNotExists()
            .WithCreateDestinationThrows<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<Exception>()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedDestinationCreation()
            .VerifyAttemptedToDownload(0);
    }

    [Fact]
    public async Task Download_Failed()
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithADestination()
            .WithFailedToDownload();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<HttpRequestException>()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload();
    }

    [Fact]
    public async Task Download_Failed_ToWriteToFile()
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithADestination()
            .WithSuccessfulDownload()
            .WithWriteToFileThrows<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<Exception>()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload()
            .VerifyAttemptedToWriteToFile();
    }

    [Fact]
    public async Task Download_Successfully()
    {
        // -- Arrange --
        using var testData = new DownloadTaskTestData(_logger)
            .WithAValidUri()
            .WithADestination()
            .WithSuccessfulDownload();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageEmpty()
            .IsResultsSuccessful()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyAttemptedToDownload()
            .VerifyAttemptedToWriteToFile();
    }
}
