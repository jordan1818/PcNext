using Asys.System;

namespace ChocolateySharp.Options;

/// <summary>
/// A definition of <see cref="ChocolateyPackageInstallOptions"/> whcih
/// represents the options of installing a Chocolatey package.
/// </summary>
public class ChocolateyPackageInstallOptions
{
    /// <summary>
    /// A speicific version to install of the package.
    /// If set to null, it will install the latest 
    /// version of the package.
    /// Default is null.
    /// </summary>
    public SemanticVersion? Version { get; set; }

    /// <summary>
    /// The package parameters for the package.
    /// View package details to see how to use
    /// for their needs.
    /// Default is null.
    /// </summary>
    public string? PackageParameters { get; set; }
}
