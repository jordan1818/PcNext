using Asys.System.Diagnostics;
using Moq;

namespace Asys.Mocks.System.Diagnostics;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IProcess}"/> instance.
/// </summary>
public static class MockProcessExtensions
{
    /// <summary>
    /// Setups a successful <see cref="Mock{IProcess}"/> instance.
    /// </summary>
    /// <param name="mockProcess">The instance of the <see cref="Mock{IProcess}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IProcess}"/> to allow for chainning.</returns>
    public static Mock<IProcess> WithSuccess(this Mock<IProcess> mockProcess)
    {
        mockProcess.Setup(m => m.Start())
            .Returns(true);
        mockProcess.Setup(m => m.WaitForExitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        mockProcess.SetupGet(m => m.HasExited)
            .Returns(true);
        mockProcess.Setup(m => m.Kill(It.IsAny<bool>()))
            .Callback<bool>(s => { });
        return mockProcess;
    }

    /// <summary>
    /// Setups a failed to start <see cref="Mock{IProcess}"/> instance.
    /// </summary>
    /// <param name="mockProcess">The instance of the <see cref="Mock{IProcess}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IProcess}"/> to allow for chainning.</returns>
    public static Mock<IProcess> WithFailedToStart(this Mock<IProcess> mockProcess)
    {
        mockProcess.Setup(m => m.Start())
                .Returns(false);
        return mockProcess;
    }

    /// <summary>
    /// Setups a throw on start <see cref="Mock{IProcess}"/> instance.
    /// </summary>
    /// <param name="mockProcess">The instance of the <see cref="Mock{IProcess}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IProcess}"/> to allow for chainning.</returns>
    public static Mock<IProcess> WithThrowsOnStart<TException>(this Mock<IProcess> mockProcess)
        where TException : Exception, new()
    {
        mockProcess.Setup(m => m.Start())
                .Throws<TException>();
        return mockProcess;
    }

    /// <summary>
    /// Setups a throw on waiting for exit <see cref="Mock{IProcess}"/> instance.
    /// </summary>
    /// <typeparam name="TException">The <see cref="Exception"/> type to throw, using default, empty, constructor</typeparam>
    /// <param name="mockProcess">The instance of the <see cref="Mock{IProcess}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IProcess}"/> to allow for chainning.</returns>
    public static Mock<IProcess> WithThrowsOnWaitingForExit<TException>(this Mock<IProcess> mockProcess)
        where TException : Exception, new()
    {
        mockProcess.WithThrowsOnWaitingForExit(new TException());
        return mockProcess;
    }

    /// <summary>
    /// Setups a throw on waiting for exit <see cref="Mock{IProcess}"/> instance.
    /// </summary>
    /// <param name="mockProcess">The instance of the <see cref="Mock{IProcess}"/> to configure.</param>
    /// <param name="exception">The instance of the <see cref="Exception"/> to throw.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IProcess}"/> to allow for chainning.</returns>
    public static Mock<IProcess> WithThrowsOnWaitingForExit(this Mock<IProcess> mockProcess, Exception exception)
    {
        mockProcess
            .Setup(m => m.WaitForExitAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        return mockProcess;
    }
}
