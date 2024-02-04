using Asys.System.IO;
using Asys.System.Net.Http;
using Asys.Mocks.System.Net.Http;
using Asys.Tests.Framework.Logger;
using Moq;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;
using HttpMessageHandler = Asys.System.Net.Http.HttpMessageHandler;
using System.Net;
using Asys.Mocks.System.IO;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal sealed class DownloadTaskTestData : TaskConfigurationTestData<DownloadTask, DownloadTaskTestData>
{
    public DownloadTaskTestData(ITestLogger logger)
        : base(logger)
    {
        MockFileSystem = new Mock<IFileSystem>();
        MockHttpMessageHandler = new Mock<IHttpMessageHandler>();
        HttpClient = new HttpClient(new HttpMessageHandler(MockHttpMessageHandler.Object));

        // Default behaviour.
        MockFileSystem.OnFileExists(false);
        MockFileSystem.OnDirectoryExists(true);
    }

    private Mock<IFileSystem> MockFileSystem { get; }

    private Mock<IHttpMessageHandler> MockHttpMessageHandler { get; }

    private HttpClient HttpClient { get; }

    protected override DownloadTask CreateTask(TaskConfiguration taskConfiguration) => new (taskConfiguration, HttpClient, MockFileSystem.Object);

    public DownloadTaskTestData WithUri(string uri) => WithProperty(nameof(DownloadTask.Uri), uri);

    public DownloadTaskTestData WithDestination(string destination) => WithProperty(nameof(DownloadTask.Destination), destination);

    public DownloadTaskTestData WithAValidUri() => WithUri("http://mock-uri.com/mock_file.txt");

    public DownloadTaskTestData WithADestination() => WithDestination("C:\\mock_path");

    public DownloadTaskTestData WithOverwriteFile() => WithProperty(nameof(DownloadTask.OverwriteFile), bool.TrueString);

    public DownloadTaskTestData WithFileAlreadyExists()
    {
        MockFileSystem.OnFileExists(true);
        return this;
    }

    public DownloadTaskTestData WithDestinationDoesNotExists()
    {
        MockFileSystem.OnDirectoryExists(false);
        return this;
    }

    public DownloadTaskTestData WithDeleteFileThrows<TException>()
        where TException : Exception, new()
    {
        MockFileSystem.ThrowsOnDeleteFile<TException>();
        return this;
    }

    public DownloadTaskTestData WithCreateDestinationThrows<TException>()
        where TException : Exception, new()
    {
        MockFileSystem.ThrowsOnCreateDirectory<TException>();
        return this;
    }

    public DownloadTaskTestData WithSuccessfulDownload()
    {
        MockHttpMessageHandler.OnSendAsyncReturns(HttpStatusCode.OK);
        return this;
    }

    public DownloadTaskTestData WithFailedToDownload()
    {
        MockHttpMessageHandler.OnSendAsyncReturns(HttpStatusCode.BadRequest);
        return this;
    }

    public DownloadTaskTestData WithWriteToFileThrows<TException>()
        where TException : Exception, new()
    {
        MockFileSystem.ThrowsOnAppendFileAsync<TException>();
        return this;
    }

    public DownloadTaskTestData VerifyAttemptedFileExists()
    {
        MockFileSystem.VerifyOnFileExists(Times.Once);
        return this;
    }

    public DownloadTaskTestData VerifyAttemptedDestinationCreation()
    {
        MockFileSystem.VerifyOnCreateDirectory(Times.Once);
        return this;
    }

    public DownloadTaskTestData VerifyAttemptedFileDeletion()
    {
        MockFileSystem.VerifyDeleteFile(Times.Once);
        return this;
    }

    public DownloadTaskTestData VerifyAttemptedToDownload(int count = 1)
    {
        MockHttpMessageHandler.VerifySendAsync(Times.Exactly(count));
        return this;
    }

    public DownloadTaskTestData VerifyAttemptedToWriteToFile(int count = 1)
    {
        MockFileSystem.VerifyAppendFileAsync(Times.Exactly(count));
        return this;
    }

    public override void Dispose()
    {
        base.Dispose();

        HttpClient?.Dispose();
    }
}
