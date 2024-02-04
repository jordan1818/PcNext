using Asys.Mocks.System.Diagnostics;
using Asys.System.Diagnostics;
using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;
using Asys.Tests.Framework.Data.Tasks;
using Asys.Tests.Framework.Logger;
using Moq;

namespace Asys.Tests.Unit.Tasks.Process;

internal sealed class ProcessTaskTestData : TaskTestData<ProcessTask, ProcessTaskContext, ProcessTaskResults, ProcessTaskTestData>
{
    public ProcessTaskTestData(ITestLogger logger)
    {
        Context = new ProcessTaskContext(logger, nameof(ProcessTaskTestData));

        MockProcessFactory = new Mock<IProcessFactory>();
        MockProcess = new Mock<IProcess>();

        MockProcessFactory.Setup(m => m.Create(It.IsAny<ProcessInformation>()))
            .Returns<ProcessInformation>(i => MockProcess.Object);
    }

    private Mock<IProcessFactory> MockProcessFactory { get; }

    private Mock<IProcess> MockProcess { get; }

    protected override ProcessTaskContext Context { get; set; }

    protected override ProcessTask CreateTask() => new(MockProcessFactory.Object);

    public ProcessTaskTestData WithSuccessfulProcess()
    {
        MockProcess.WithSuccess();
        return this;
    }

    public ProcessTaskTestData WithProcessFailedToStart()
    {
        MockProcess
            .WithFailedToStart();
        return this;
    }

    public ProcessTaskTestData WithProcessThrowsOnStart<TException>()
        where TException : Exception, new()
    {
        MockProcess
            .WithThrowsOnStart<TException>();
        return this;
    }

    public ProcessTaskTestData WithProcessThrowsOnWaitingForExit<TException>()
        where TException : Exception, new()
    {
        MockProcess
            .WithThrowsOnWaitingForExit<TException>();
        return this;
    }

    public ProcessTaskTestData WithProcessThrowsOnWaitingForExit(Exception exception)
    {
        MockProcess
            .WithThrowsOnWaitingForExit(exception);
        return this;
    }

    public ProcessTaskTestData WithProcessStillAlive()
    {
        MockProcess
            .SetupGet(p => p.HasExited)
                .Returns(false);
        return this;
    }

    public ProcessTaskTestData WithProcessExitCode(int exitCode)
    {
        MockProcess
            .SetupGet(p => p.ExitCode)
                .Returns(exitCode);
        return this;
    }

    public ProcessTaskTestData VerifyProcessAttemptedToStart(int attemptCount = 1)
    {
        MockProcess.Verify(m => m.Start(), Times.Exactly(attemptCount));
        return this;
    }

    public ProcessTaskTestData VerifyProcessAttemptedToWaitForExit(int attemptCount = 1)
    {
        MockProcess.Verify(m => m.WaitForExitAsync(It.IsAny<CancellationToken>()), Times.Exactly(attemptCount));
        return this;
    }

    public ProcessTaskTestData VerifyProcessAttemptedToKill(int attemptCount = 1)
    {
        MockProcess.Verify(m => m.Kill(It.IsAny<bool>()), Times.Exactly(attemptCount));
        return this;
    }
}
