using Asys.Mocks.System.Environment;
using Asys.Mocks.System.Environment.Windows.Scheduler;
using Asys.Mocks.System.Security;
using Asys.System.Environment;
using Asys.System.Environment.Windows.Scheduler;
using Asys.System.Security;
using Asys.Tests.Framework.Logger;
using Moq;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal sealed class OnStartUpTaskTestData : TaskConfigurationTestData<OnStartUpTask, OnStartUpTaskTestData>
{
    public OnStartUpTaskTestData(ITestLogger logger)
        : base(logger)
    {
        MockTaskFolder = new Mock<ITaskFolder>();
        MockOperatingSystem = new Mock<IOperatingSystem>();
        MockAccountManager = new Mock<IAccountManager>();

        // Ensure default behaviour
        WithValidSetup();
    }

    private Mock<ITaskFolder> MockTaskFolder { get; }

    private Mock<IOperatingSystem> MockOperatingSystem { get; }

    private Mock<IAccountManager> MockAccountManager { get; }

    protected override OnStartUpTask CreateTask(TaskConfiguration taskConfiguration)
    {
        var mockTaskScheduler = new Mock<ITaskScheduler>();
        mockTaskScheduler.SetupGet(p => p.RootFolder)
            .Returns(() => MockTaskFolder.Object);

        return new(taskConfiguration, mockTaskScheduler.Object, MockOperatingSystem.Object, MockAccountManager.Object);
    }

    public OnStartUpTaskTestData WithValidSetup()
        => WithFoundAccount()
        .WithOsIsWindows()
        .WithAValidFolder()
        .WithAValidCommand();

    public OnStartUpTaskTestData WithFolder(string folder) => WithProperty(nameof(OnStartUpTask.Folder), folder);

    public OnStartUpTaskTestData WithCommand(string command) => WithProperty(nameof(OnStartUpTask.Command), command);

    public OnStartUpTaskTestData WithAValidFolder() => WithFolder("MockFolder");

    public OnStartUpTaskTestData WithAInvalidFolder() => WithFolder(string.Empty);

    public OnStartUpTaskTestData WithAValidCommand() => WithCommand("MockCommand");

    public OnStartUpTaskTestData WithAInvalidCommand() => WithCommand(string.Empty);

    public OnStartUpTaskTestData WithFoundAccount()
    {
        MockAccountManager.GetCurrentIdentityAsEmpty();
        return this;
    }

    public OnStartUpTaskTestData WithOsIsWindows()
    {
        MockOperatingSystem.OnIsWindows(true);
        return this;
    }

    public OnStartUpTaskTestData WithOsIsNotWindows()
    {
        MockOperatingSystem.OnIsWindows(false);
        return this;
    }

    public OnStartUpTaskTestData WithNotFoundAccount()
    {
        MockAccountManager.GetCurrentIdentityAsNull();
        return this;
    }

    public OnStartUpTaskTestData WithAccountRetrievalThrows<TException>()
        where TException : Exception, new()
    {
        MockAccountManager.ThrowsOnGetCurrentIdentity<TException>();
        return this;
    }

    public OnStartUpTaskTestData VerifyStartupTaskWasRegistered(int count = 1)
    {
        MockTaskFolder.VerifyRegister(Times.Exactly(count));
        return this;
    }
}
