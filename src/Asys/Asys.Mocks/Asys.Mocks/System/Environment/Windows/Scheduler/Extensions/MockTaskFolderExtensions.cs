using Asys.System.Environment.Windows.Scheduler;
using Moq;

namespace Asys.Mocks.System.Environment.Windows.Scheduler;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{ITaskFolder}"/> instance.
/// </summary>
public static class MockTaskFolderExtensions
{
    /// <summary>
    /// Setups a throw on a mocked <see cref="ITaskFolder.Register(string, TaskDefinition)"/>.
    /// </summary>
    /// <param name="mockTaskFolder">The instance of the <see cref="Mock{ITaskFolder}"/> to configure.</param>
    /// <param name="exception">The <see cref="Exception"/> instance that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITaskFolder}"/> to allow for chainning.</returns>
    public static Mock<ITaskFolder> ThrowsOnRegister(this Mock<ITaskFolder> mockTaskFolder, Exception exception)
    {
        mockTaskFolder.Setup(m => m.Register(It.IsAny<string>(), It.IsAny<TaskDefinition>()))
                .Throws(exception);
        return mockTaskFolder;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="ITaskFolder.Register(string, TaskDefinition)"/>.
    /// </summary>
    /// <param name="mockTaskFolder">The instance of the <see cref="Mock{ITaskFolder}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITaskFolder}"/> to allow for chainning.</returns>
    public static Mock<ITaskFolder> ThrowsOnRegister<TException>(this Mock<ITaskFolder> mockTaskFolder)
        where TException : Exception, new()
    => mockTaskFolder.ThrowsOnRegister(new TException());

    /// <summary>
    /// Verifies the mocked <see cref="ITaskFolder.Register(string, TaskDefinition)"/> has been called.
    /// </summary>
    /// <param name="mockTaskFolder">The instance of the <see cref="Mock{ITaskFolder}"/> to verify.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITaskFolder}"/> to allow for chainning.</returns>
    public static Mock<ITaskFolder> VerifyRegister(this Mock<ITaskFolder> mockTaskFolder, Times times)
    {
        mockTaskFolder.Verify(m => m.Register(It.IsAny<string>(), It.IsAny<TaskDefinition>()), times);
        return mockTaskFolder;
    }
}