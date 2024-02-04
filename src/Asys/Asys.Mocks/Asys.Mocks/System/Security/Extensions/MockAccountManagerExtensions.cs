using Asys.System.Security;
using Moq;

namespace Asys.Mocks.System.Security;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IAccountManager}"/> instance.
/// </summary>
public static class MockAccountManagerExtensions
{
    /// <summary>
    /// Setups mocked <see cref="IAccountManager.GetCurrentIdentity()"/> to return a result of the specified <see cref="IAccountIdentity"/>.
    /// </summary>
    /// <param name="mockAccountManager">The instance of the <see cref="Mock{IAccountManager}"/> to configure.</param>
    /// <param name="accountIdentity">The instance of <see cref="IAccountIdentity"/> to be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IAccountManager}"/> to allow for chainning.</returns>
    public static Mock<IAccountManager> OnGetCurrentIdentity(this Mock<IAccountManager> mockAccountManager, IAccountIdentity? accountIdentity)
    {
        mockAccountManager.Setup(m => m.GetCurrentIdentity())
            .Returns(() => accountIdentity);
        return mockAccountManager;
    }

    /// <summary>
    /// Setups mocked <see cref="IAccountManager.GetCurrentIdentity()"/> to return a null result of <see cref="IAccountIdentity"/>..
    /// </summary>
    /// <param name="mockAccountManager">The instance of the <see cref="Mock{IAccountManager}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IAccountManager}"/> to allow for chainning.</returns>
    public static Mock<IAccountManager> GetCurrentIdentityAsNull(this Mock<IAccountManager> mockAccountManager)
        => mockAccountManager.OnGetCurrentIdentity(accountIdentity: null);

    /// <summary>
    /// Setups mocked <see cref="IAccountManager.GetCurrentIdentity()"/> to return an empty result of <see cref="IAccountIdentity"/>.
    /// </summary>
    /// <param name="mockAccountManager">The instance of the <see cref="Mock{IAccountManager}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IAccountManager}"/> to allow for chainning.</returns>
    public static Mock<IAccountManager> GetCurrentIdentityAsEmpty(this Mock<IAccountManager> mockAccountManager)
        => mockAccountManager.OnGetCurrentIdentity(new Mock<IAccountIdentity>().Object);

    /// <summary>
    /// Setups a throw on a mocked <see cref="IAccountManager.GetCurrentIdentity()"/>.
    /// </summary>
    /// <param name="mockAccountManager">The instance of the <see cref="Mock{IAccountManager}"/> to configure.</param>
    /// <param name="exception">The <see cref="Exception"/> instance that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IAccountManager}"/> to allow for chainning.</returns>
    public static Mock<IAccountManager> ThrowsOnGetCurrentIdentity(this Mock<IAccountManager> mockAccountManager, Exception exception)
    {
        mockAccountManager.Setup(m => m.GetCurrentIdentity())
                .Throws(exception);
        return mockAccountManager;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="IAccountManager.GetCurrentIdentity()"/>.
    /// </summary>
    /// <param name="mockAccountManager">The instance of the <see cref="Mock{IAccountManager}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IAccountManager}"/> to allow for chainning.</returns>
    public static Mock<IAccountManager> ThrowsOnGetCurrentIdentity<TException>(this Mock<IAccountManager> mockAccountManager)
        where TException : Exception, new()
    => mockAccountManager.ThrowsOnGetCurrentIdentity(new TException());
}
