using ChocolateySharp.Options;
using Moq;

namespace ChocolateySharp.Mocks;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IChocolateyClient}"/> instance.
/// </summary>
public static class MockChocolateyClientExtensions
{
    /// <summary>
    /// Setups a <see cref="Mock{IChocolateyClient}"/> instance for <see cref="IChocolateyClient.InstallPackageAsync(string, ChocolateyPackageInstallOptions?, CancellationToken)"/> a result.
    /// </summary>
    /// <param name="mockChocolateyClient">The instance of the <see cref="Mock{IChocolateyClient}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IChocolateyClient}"/> to allow for chainning.</returns>
    public static Mock<IChocolateyClient> WithPackageInstallResult(this Mock<IChocolateyClient> mockChocolateyClient, bool result)
    {
        mockChocolateyClient.Setup(m => m.InstallPackageAsync(It.IsAny<string>(), It.IsAny<ChocolateyPackageInstallOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>  result);

        return mockChocolateyClient;
    }

    /// <summary>
    /// Setups a <see cref="Mock{IChocolateyClient}"/> instance for <see cref="IChocolateyClient.InstallPackageAsync(string, ChocolateyPackageInstallOptions?, CancellationToken)"/> with a successful result.
    /// </summary>
    /// <param name="mockChocolateyClient">The instance of the <see cref="Mock{IChocolateyClient}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IChocolateyClient}"/> to allow for chainning.</returns>
    public static Mock<IChocolateyClient> WithSuccessfullyPackageInstall(this Mock<IChocolateyClient> mockChocolateyClient) => mockChocolateyClient.WithPackageInstallResult(result: true);

    /// <summary>
    /// Setups a <see cref="Mock{IChocolateyClient}"/> instance for <see cref="IChocolateyClient.InstallPackageAsync(string, ChocolateyPackageInstallOptions?, CancellationToken)"/> with a failed result.
    /// </summary>
    /// <param name="mockChocolateyClient">The instance of the <see cref="Mock{IChocolateyClient}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IChocolateyClient}"/> to allow for chainning.</returns>
    public static Mock<IChocolateyClient> WithFailedToInstallPackage(this Mock<IChocolateyClient> mockChocolateyClient) => mockChocolateyClient.WithPackageInstallResult(result: false);

    /// <summary>
    /// Setups a <see cref="Mock{IChocolateyClient}"/> instance for <see cref="IChocolateyClient.InstallPackageAsync(string, ChocolateyPackageInstallOptions?, CancellationToken)"/> with exception.
    /// </summary>
    /// <param name="mockChocolateyClient">The instance of the <see cref="Mock{IChocolateyClient}"/> to configure.</param>
    /// <param name="exception">The exception to throw of the <see cref="Mock{IChocolateyClient}"/> instance.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IChocolateyClient}"/> to allow for chainning.</returns>
    public static Mock<IChocolateyClient> WithPackageInstallThrows(this Mock<IChocolateyClient> mockChocolateyClient, Exception exception)
    {
        mockChocolateyClient.Setup(m => m.InstallPackageAsync(It.IsAny<string>(), It.IsAny<ChocolateyPackageInstallOptions>(), It.IsAny<CancellationToken>()))
            .Throws(() => exception);

        return mockChocolateyClient;
    }

    /// <summary>
    /// Setups a <see cref="Mock{IChocolateyClient}"/> instance for <see cref="IChocolateyClient.InstallPackageAsync(string, ChocolateyPackageInstallOptions?, CancellationToken)"/> with exception.
    /// </summary>
    /// <param name="mockChocolateyClient">The instance of the <see cref="Mock{IChocolateyClient}"/> to configure.</param>
    /// <typeparam name="TException">The type of <see cref="Exception"/> using it's default constructor.</typeparam>
    /// <returns>The confgiured instane of <see cref="Mock{IChocolateyClient}"/> to allow for chainning.</returns>
    public static Mock<IChocolateyClient> WithPackageInstallThrows<TException>(this Mock<IChocolateyClient> mockChocolateyClient)
        where TException : Exception, new() 
        => mockChocolateyClient.WithPackageInstallThrows(new TException());
}
