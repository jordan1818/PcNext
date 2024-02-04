using ChocolateySharp.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ChocolateySharp.Tests")]

namespace ChocolateySharp;

/// <summary>
/// A definition of <see cref="IChocolateyClient"/>.
/// </summary>
public interface IChocolateyClient
{
    /// <summary>
    /// Installs a Chocolatey package.
    /// </summary>
    /// <param name="name">The name of the Chocolatey package.</param>
    /// <param name="options">An instance of <see cref="ChocolateyPackageInstallOptions"/> for options of install the Chocolatey package of <paramref name="name"/>.</param>
    /// <param name="cancellationToken">A cancellation token for the installion of the package.</param>
    /// <returns>True if the package was successfully installed; otherwise false.</returns>
    Task<bool> InstallPackageAsync(string name, ChocolateyPackageInstallOptions? options = null, CancellationToken cancellationToken = default);
}