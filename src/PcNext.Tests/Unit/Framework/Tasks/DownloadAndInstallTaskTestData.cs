using Asys.Mocks.System.IO;
using Asys.Mocks.Tasks;
using Asys.Mocks.Tasks.Process;
using Asys.System.IO;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tests.Framework.Logger;
using Moq;
using PcNext.Framework;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal sealed class DownloadAndInstallTaskTestData : TaskConfigurationTestData<DownloadAndInstallTask, DownloadAndInstallTaskTestData>
{
    public DownloadAndInstallTaskTestData(ITestLogger logger)
        : base(logger)
    {
        MockDownloadTask = new Mock<ITask>();
        MockInstallTask = new Mock<ProcessTask>();
    }

    private Mock<ITask> MockDownloadTask { get; }

    private Mock<ProcessTask> MockInstallTask { get; }

    protected override DownloadAndInstallTask CreateTask(TaskConfiguration taskConfiguration)
    {
        var mockFileSystem = new Mock<IFileSystem>();
        var mockTaskConfigurationTaskFactory = new Mock<ITaskConfigurationTaskFactory>();
        var mockProcessTaskFactory = new Mock<IProcessTaskFactory>();

        mockFileSystem
            .OnCreateTemporaryDirectory(new Mock<ITemporaryDirectory>().AsEmpty().Object);

        mockTaskConfigurationTaskFactory.Setup(m => m.Create(It.IsAny<TaskConfiguration>()))
            .Returns<TaskConfiguration>(c => MockDownloadTask.Object);

        mockProcessTaskFactory.Setup(m => m.Create())
            .Returns(() => MockInstallTask.Object);

        return new(taskConfiguration, mockFileSystem.Object, mockTaskConfigurationTaskFactory.Object, mockProcessTaskFactory.Object);
    }

    public DownloadAndInstallTaskTestData WithInstallerUri(string uri) => WithProperty(nameof(DownloadAndInstallTask.InstallerUri), uri);

    public DownloadAndInstallTaskTestData WithValidInstallerUri() => WithInstallerUri("http://mock-uri.com/mock-installer.exe");

    public DownloadAndInstallTaskTestData WithSuccessfulDownload()
    {
        MockDownloadTask.WithSuccessfulResults();
        return this;
    }

    public DownloadAndInstallTaskTestData WithFailedToDownload()
    {
        MockDownloadTask.WithFailureResults();
        return this;
    }

    public DownloadAndInstallTaskTestData WithDownloadThrows<TException>()
        where TException : Exception, new()
    {
        MockDownloadTask.Throws<TException>();
        return this;
    }

    public DownloadAndInstallTaskTestData WithSuccessfulInstall()
    {
        MockInstallTask.WithSuccessfulResults();
        return this;
    }

    public DownloadAndInstallTaskTestData WithFailedToInstall()
    {
        MockInstallTask.WithFailureResults();
        return this;
    }

    public DownloadAndInstallTaskTestData WithInstallThrows<TException>()
        where TException : Exception, new()
    {
        MockInstallTask.Throws<TException>();
        return this;
    }

    public DownloadAndInstallTaskTestData VerifyAttemptedToDownload(int count = 1)
    {
        MockDownloadTask.Verify(m => m.ExecuteAsync(It.IsAny<TaskContext>()), Times.Exactly(count));
        return this;
    }

    public DownloadAndInstallTaskTestData VerifyAttemptedToInstall(int count = 1)
    {
        MockInstallTask.Verify(m => m.ExecuteAsync(It.IsAny<ProcessTaskContext>()), Times.Exactly(count));
        return this;
    }
}
