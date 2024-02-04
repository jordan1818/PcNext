using Asys.System.Environment;
using Moq;

namespace Asys.Mocks.System.Environment;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IOperatingSystem}"/> instance.
/// </summary>
public static class MockOperatingSystemExtensions
{
    /// <summary>
    /// Setups mocked <see cref="IOperatingSystem.IsWindows()"/> to return a result.
    /// </summary>
    /// <param name="mockOperatingSystem">The instance of the <see cref="Mock{IOperatingSystem}"/> to configure.</param>
    /// <param name="result">The result to be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IOperatingSystem}"/> to allow for chainning.</returns>
    public static Mock<IOperatingSystem> OnIsWindows(this Mock<IOperatingSystem> mockOperatingSystem, bool result)
    {
        mockOperatingSystem.Setup(m => m.IsWindows())
            .Returns(() => result);
        return mockOperatingSystem;
    }
}
