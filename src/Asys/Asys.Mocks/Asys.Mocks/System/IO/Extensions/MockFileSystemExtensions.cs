using Asys.System.IO;
using Asys.System.Net.Http;
using Moq;
using System.Runtime.CompilerServices;

namespace Asys.Mocks.System.IO;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IFileSystem}"/> instance.
/// </summary>
public static class MockFileSystemExtensions
{
    /// <summary>
    /// Setups <see cref="IFileSystem.CreateTemporaryDirectory(string?)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="createTemporaryDirectoy">The function to execute on <see cref="IFileSystem.CreateTemporaryDirectory(string?)"/> method to return and instance of <see cref="ITemporaryDirectory"/>.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> OnCreateTemporaryDirectory(this Mock<IFileSystem> mockFileSystem, Func<string, ITemporaryDirectory> createTemporaryDirectoy)
    {
        mockFileSystem.Setup(m => m.CreateTemporaryDirectory(It.IsAny<string>()))
            .Returns<string>(p => createTemporaryDirectoy(p));
        return mockFileSystem;
    }

    /// <summary>
    /// Setups <see cref="IFileSystem.CreateTemporaryDirectory(string?)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="temporaryDirectory">The instance of <see cref="ITemporaryDirectory"/> to be returned from <see cref="IFileSystem.CreateTemporaryDirectory(string?)"/> method.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> OnCreateTemporaryDirectory(this Mock<IFileSystem> mockFileSystem, ITemporaryDirectory temporaryDirectory) 
        => mockFileSystem.OnCreateTemporaryDirectory(_ => temporaryDirectory);

    /// <summary>
    /// Setups <see cref="IFileSystem.FileExists(string)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="result">The result to be returned from the method.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> OnFileExists(this Mock<IFileSystem> mockFileSystem, bool result)
    {
        mockFileSystem.Setup(m => m.FileExists(It.IsAny<string>()))
            .Returns(result);
        return mockFileSystem;
    }

    /// <summary>
    /// Setups <see cref="IFileSystem.DirectoryExists(string)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="result">The result to be returned from the method.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> OnDirectoryExists(this Mock<IFileSystem> mockFileSystem, bool result)
    {
        mockFileSystem.Setup(m => m.DirectoryExists(It.IsAny<string>()))
            .Returns(result);
        return mockFileSystem;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="IFileSystem.DeleteFile(string)"/>.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="exception">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> ThrowsOnDeleteFile(this Mock<IFileSystem> mockFileSystem, Exception exception)
    {
        mockFileSystem.Setup(m => m.DeleteFile(It.IsAny<string>()))
            .Throws(exception);
        return mockFileSystem;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="IFileSystem.DeleteFile(string)"/>.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> ThrowsOnDeleteFile<TException>(this Mock<IFileSystem> mockFileSystem)
        where TException : Exception, new()
    => mockFileSystem.ThrowsOnDeleteFile(new TException());

    /// <summary>
    /// Setups a throw on a mocked <see cref="IFileSystem.CreateDirectory(string)"/>.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="exception">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> ThrowsOnCreateDirectory(this Mock<IFileSystem> mockFileSystem, Exception exception)
    {
        mockFileSystem.Setup(m => m.CreateDirectory(It.IsAny<string>()))
            .Throws(exception);
        return mockFileSystem;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="IFileSystem.CreateDirectory(string)"/>.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> ThrowsOnCreateDirectory<TException>(this Mock<IFileSystem> mockFileSystem)
        where TException : Exception, new()
    => mockFileSystem.ThrowsOnCreateDirectory(new TException());

    /// <summary>
    /// Setups a throw on a mocked <see cref="IFileSystem.AppendFileAsync(string, string?, CancellationToken)"/>.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="exception">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> ThrowsOnAppendFileAsync(this Mock<IFileSystem> mockFileSystem, Exception exception)
    {
        mockFileSystem.Setup(m => m.AppendFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
        return mockFileSystem;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="IFileSystem.AppendFileAsync(string, string?, CancellationToken)"/>.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> ThrowsOnAppendFileAsync<TException>(this Mock<IFileSystem> mockFileSystem)
        where TException : Exception, new()
    => mockFileSystem.ThrowsOnAppendFileAsync(new TException());

    /// <summary>
    /// Verifies <see cref="IFileSystem.FileExists(string)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> VerifyOnFileExists(this Mock<IFileSystem> mockFileSystem, Func<Times> times)
    {
        mockFileSystem.Verify(m => m.FileExists(It.IsAny<string>()), times);
        return mockFileSystem;
    }

    /// <summary>
    /// Verifies <see cref="IFileSystem.CreateDirectory(string)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> VerifyOnCreateDirectory(this Mock<IFileSystem> mockFileSystem, Func<Times> times)
    {
        mockFileSystem.Verify(m => m.CreateDirectory(It.IsAny<string>()), times);
        return mockFileSystem;
    }

    /// <summary>
    /// Verifies <see cref="IFileSystem.DeleteFile(string)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> VerifyDeleteFile(this Mock<IFileSystem> mockFileSystem, Func<Times> times)
    {
        mockFileSystem.Verify(m => m.DeleteFile(It.IsAny<string>()), times);
        return mockFileSystem;
    }

    /// <summary>
    /// Verifies <see cref="IFileSystem.AppendFileAsync(string, string?, CancellationToken)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> VerifyAppendFileAsync(this Mock<IFileSystem> mockFileSystem, Times times)
    {
        mockFileSystem.Verify(m => m.AppendFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), times);
        return mockFileSystem;
    }

    /// <summary>
    /// Verifies <see cref="IFileSystem.AppendFileAsync(string, string?, CancellationToken)"/> of <see cref="Mock{IFileSystem}"/> instance.
    /// </summary>
    /// <param name="mockFileSystem">The instance of the <see cref="Mock{IFileSystem}"/> to configure.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IFileSystem}"/> to allow for chainning.</returns>
    public static Mock<IFileSystem> VerifyAppendFileAsync(this Mock<IFileSystem> mockFileSystem, Func<Times> times)
    {
        mockFileSystem.Verify(m => m.AppendFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), times);
        return mockFileSystem;
    }
}
