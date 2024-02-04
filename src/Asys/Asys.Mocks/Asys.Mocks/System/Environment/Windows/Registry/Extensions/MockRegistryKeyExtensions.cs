using Asys.System.Environment.Windows.Registry;
using Moq;

namespace Asys.Mocks.System.Environment.Windows.Registry;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IRegistryKey}"/> instance.
/// </summary>
public static class MockRegistryKeyExtensions
{
    /// <summary>
    /// Setups mocked <see cref="IRegistryKey.CreateOrOpenSubKey(string, RegistryKeyPermissionCheck)"/> of <see cref="Mock{IRegistryKey}"/> instance to return a result.
    /// </summary>
    /// <param name="mockRegistryKey">The instance of the <see cref="Mock{IRegistryKey}"/> to configure.</param>
    /// <param name="onReturn">The lambda that will return a instance of <see cref="Func{IRegistryKey}"/>.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistryKey}"/> to allow for chainning.</returns>
    public static Mock<IRegistryKey> ReturnOnCreateOrOpenSubKey(this Mock<IRegistryKey> mockRegistryKey, Func<IRegistryKey?> onReturn)
    {
        mockRegistryKey.Setup(m => m.CreateOrOpenSubKey(It.IsAny<string>(), It.IsAny<RegistryKeyPermissionCheck>()))
            .Returns(() => onReturn());
        return mockRegistryKey;
    }

    /// <summary>
    /// Setups mocked <see cref="IRegistryKey.CreateOrOpenSubKey(string, RegistryKeyPermissionCheck)"/> of <see cref="Mock{IRegistryKey}"/> instance to return a result.
    /// </summary>
    /// <param name="mockRegistryKey">The instance of the <see cref="Mock{IRegistryKey}"/> to configure.</param>
    /// <param name="result">The instance of <see cref="IRegistryKey"/> that will be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistryKey}"/> to allow for chainning.</returns>
    public static Mock<IRegistryKey> ReturnOnCreateOrOpenSubKey(this Mock<IRegistryKey> mockRegistryKey, IRegistryKey? result)
        => mockRegistryKey.ReturnOnCreateOrOpenSubKey(() => result);

    /// <summary>
    /// Setup a throw on a mocked <see cref="IRegistryKey.CreateOrOpenSubKey(string, RegistryKeyPermissionCheck)"/> of <see cref="Mock{IRegistryKey}"/> instance.
    /// </summary>
    /// <param name="mockRegistryKey">The instance of the <see cref="Mock{IRegistryKey}"/> to configure.</param>
    /// <param name="exception">The instance of <see cref="Exception"/> that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistryKey}"/> to allow for chainning.</returns>
    public static Mock<IRegistryKey> ThrowsOnCreateOrOpenSubKey(this Mock<IRegistryKey> mockRegistryKey, Exception exception)
    {
        mockRegistryKey.Setup(m => m.CreateOrOpenSubKey(It.IsAny<string>(), It.IsAny<RegistryKeyPermissionCheck>()))
            .Throws(() => exception);
        return mockRegistryKey;
    }

    /// <summary>
    /// Setup a throw on a mocked <see cref="IRegistryKey.CreateOrOpenSubKey(string, RegistryKeyPermissionCheck)"/> of <see cref="Mock{IRegistryKey}"/> instance.
    /// </summary>
    /// <param name="mockRegistryKey">The instance of the <see cref="Mock{IRegistryKey}"/> to configure.</param>
    /// <typeparam name="TException">The type of <see cref="Exception"/> that will be thrown.</typeparam>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistryKey}"/> to allow for chainning.</returns>
    public static Mock<IRegistryKey> ThrowsOnCreateOrOpenSubKey<TException>(this Mock<IRegistryKey> mockRegistryKey)
        where TException : Exception, new()
        => mockRegistryKey.ThrowsOnCreateOrOpenSubKey(new TException());
}
