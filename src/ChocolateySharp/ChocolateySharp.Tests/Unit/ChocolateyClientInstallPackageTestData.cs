using Asys.System;
using Asys.Tests.Framework;
using Asys.Tests.Framework.Asserts;
using Asys.Tests.Framework.Logger;
using ChocolateySharp.Options;

namespace ChocolateySharp.Tests.Unit;

internal sealed class ChocolateyClientInstallPackageTestData : ChocolateyClientTestData<ChocolateyClientInstallPackageTestData>
{
    public ChocolateyClientInstallPackageTestData(ITestLogger logger) 
        : base(logger)
    {
    }

    private string Name { get; set; } = string.Empty;

    private ChocolateyPackageInstallOptions Options { get; set; } = new ChocolateyPackageInstallOptions();

    private bool WasInstalled { get; set; }

    protected override Task InternalActAsync(IChocolateyClient client, ActOptions? actOptions = null)
    {
        return Task.Run(async () =>
        {
            var installPackageTimes = actOptions?.ActTimes ?? 1;
            for (var i = 0; i < installPackageTimes; i++)
            {
                WasInstalled = await client.InstallPackageAsync(Name, Options);
            }
        });
    }

    public ChocolateyClientInstallPackageTestData WithPackageName(string name)
    {
        Name = name;
        return this;
    }

    public ChocolateyClientInstallPackageTestData WithOptions(ChocolateyPackageInstallOptions options)
    {
        Options = options;
        return this;
    }

    public ChocolateyClientInstallPackageTestData WithVersion(SemanticVersion version)
    {
        Options.Version = version;
        return this;
    }

    public ChocolateyClientInstallPackageTestData WithParameters(string packageParameters)
    {
        Options.PackageParameters = packageParameters;
        return this;
    }

    public ChocolateyClientInstallPackageTestData PackageIsInstalled()
    {
        Assert.True(WasInstalled);
        return this;
    }

    public ChocolateyClientInstallPackageTestData PackageIsNotInstalled()
    {
        Assert.False(WasInstalled);
        return this;
    }
}
