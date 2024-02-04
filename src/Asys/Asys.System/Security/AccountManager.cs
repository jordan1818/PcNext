using Asys.System.Environment;
using Asys.System.Security.Windows;

namespace Asys.System.Security;

/// <summary>
/// The implementation of <see cref="IAccountManager"/> within <see cref="AccountManager"/>.
/// </summary>
public sealed class AccountManager : IAccountManager
{
    private readonly IOperatingSystem _operatingSystem;

    /// <summary>
    /// Initalizes an instance of <see cref="AccountManager"/>.
    /// </summary>
    /// <param name="operatingSystem">The instance of <see cref="IOperatingSystem"/>. This detemines on how to retrieve operating system <see cref="IAccountIdentity"/>.</param>
    public AccountManager(IOperatingSystem operatingSystem)
    {
        _operatingSystem = operatingSystem;
    }

    /// <inheritdoc/>
    public IAccountIdentity? GetCurrentIdentity()
    {
        if (_operatingSystem.IsWindows())
        {
            return new WindowsAccountIdentity();
        }

        return null;
    }
}
