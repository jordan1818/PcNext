using Asys.System.Environment.Windows.Registry;
using Moq;

namespace Asys.Mocks.System.Environment.Windows.Registry;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IRegistry}"/> instance.
/// </summary>
public static class MockRegistryExtensions
{
    /// <summary>
    /// Setups mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance to return a result.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="hive">The <see cref="RegistryHive"/> to return for.</param>
    /// <param name="onReturn">The instance of <see cref="Func{IRegistryKey}"/> that will be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ReturnOnGet(this Mock<IRegistry> mockRegistry, RegistryHive hive, Func<IRegistryKey?> onReturn)
    {
        mockRegistry.Setup(m => m.Get(hive))
            .Returns(() => onReturn());
        return mockRegistry;
    }

    /// <summary>
    /// Setups mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance to return a result for any <see cref="RegistryHive"/>.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="onReturn">The instance of <see cref="Func{IRegistryKey}"/> that will be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ReturnOnGet(this Mock<IRegistry> mockRegistry, Func<IRegistryKey?> onReturn)
    {
        mockRegistry.Setup(m => m.Get(It.IsAny<RegistryHive>()))
            .Returns(() => onReturn());
        return mockRegistry;
    }

    /// <summary>
    /// Setups mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance to return a result.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="hive">The <see cref="RegistryHive"/> to return for.</param>
    /// <param name="result">A instance of <see cref="IRegistryKey"/> to be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ReturnOnGet(this Mock<IRegistry> mockRegistry, RegistryHive hive, IRegistryKey? result)
        => mockRegistry.ReturnOnGet(hive, () => result);

    /// <summary>
    /// Setups mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance to return a result for any <see cref="RegistryHive"/>.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="result">A instance of <see cref="IRegistryKey"/> to be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ReturnOnGet(this Mock<IRegistry> mockRegistry, IRegistryKey? result)
        => mockRegistry.ReturnOnGet(() => result);

    /// <summary>
    /// Throws on mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="hive">The <see cref="RegistryHive"/> to throw for.</param>
    /// <param name="exception">The instance of <see cref="Exception"/> that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ThrowsOnGet(this Mock<IRegistry> mockRegistry, RegistryHive hive, Exception exception)
    {
        mockRegistry.Setup(m => m.Get(hive))
            .Throws(() => exception);
        return mockRegistry;
    }

    /// <summary>
    /// Throws mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="exception">The instance of <see cref="Exception"/> that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ThrowsOnGet(this Mock<IRegistry> mockRegistry, Exception exception)
    {
        mockRegistry.Setup(m => m.Get(It.IsAny<RegistryHive>()))
            .Throws(() => exception);
        return mockRegistry;
    }

    /// <summary>
    /// Throws mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance.
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <param name="hive">The <see cref="RegistryHive"/> to throw for.</param>
    /// <typeparam name="TException">The type of <see cref="Exception"/> that will be thrown.</typeparam>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ThrowsOnGet<TException>(this Mock<IRegistry> mockRegistry, RegistryHive hive)
        where TException : Exception, new()
            => mockRegistry.ThrowsOnGet(hive, new TException());

    /// <summary>
    /// Throws mocked <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance for any <see cref="RegistryHive"/>.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to configure.</param>
    /// <typeparam name="TException">The type of <see cref="Exception"/> that will be thrown.</typeparam>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> ThrowsOnGet<TException>(this Mock<IRegistry> mockRegistry)
        where TException : Exception, new()
            => mockRegistry.ThrowsOnGet(new TException());

    /// <summary>
    /// Verifies <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to verify.</param>
    /// <param name="hive">The specific <see cref="RegistryHive"/> to verify for this method.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> VerifyOnGet(this Mock<IRegistry> mockRegistry, RegistryHive hive, Func<Times> times)
    {
        mockRegistry.Verify(m => m.Get(hive), times);
        return mockRegistry;
    }

    /// <summary>
    /// Verifies <see cref="IRegistry.Get(RegistryHive)"/> of <see cref="Mock{IRegistry}"/> instance from any <see cref="RegistryHive"/>.
    /// </summary>
    /// <param name="mockRegistry">The instance of the <see cref="Mock{IRegistry}"/> to verify.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IRegistry}"/> to allow for chainning.</returns>
    public static Mock<IRegistry> VerifyOnGet(this Mock<IRegistry> mockRegistry, Func<Times> times)
    {
        mockRegistry.Verify(m => m.Get(It.IsAny<RegistryHive>()), times);
        return mockRegistry;
    }
}
