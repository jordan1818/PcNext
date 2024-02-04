using Asys.System.IO;
using Moq;

namespace Asys.Mocks.System.IO;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{ITemporaryDirectory}"/> instance.
/// </summary>
public static class MockTemporaryDirectoryExtensions
{
    /// <summary>
    /// Setups an empty <see cref="Mock{ITemporaryDirectory}"/> instance.
    /// </summary>
    /// <param name="mockTemporaryDirectory">The instance of the <see cref="Mock{ITemporaryDirectory}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{ITemporaryDirectory}"/> to allow for chainning.</returns>
    public static Mock<ITemporaryDirectory> AsEmpty(this Mock<ITemporaryDirectory> mockTemporaryDirectory)
    {
        mockTemporaryDirectory.SetupGet(p => p.Path)
            .Returns("C:\\mock-temporary-path");

        mockTemporaryDirectory.Setup(m => m.Dispose())
            .Callback(() =>
            {
                // Empty on purpose.
            });

        return mockTemporaryDirectory;
    }
}
