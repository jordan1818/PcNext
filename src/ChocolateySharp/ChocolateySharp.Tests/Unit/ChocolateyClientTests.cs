using Asys.Tests.Framework;
using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace ChocolateySharp.Tests.Unit;

public sealed class ChocolateyClientTests
{
    private readonly ITestLogger _logger;

    public ChocolateyClientTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task InstallPackageAsync_Success_Twice_ButInstallChoco_Once()
    {
        // -- Arrange --
        var actOptions = new ActOptions
        {
            ActTimes = 2, // Install package twice.
        };

        using var testData = new ChocolateyClientInstallPackageTestData(_logger)
            .WithValidateInstalledChoco()
                .ButNoInstalledChocoFound()
            .WithInstallChoco()
            .WithExecuteChocoCommand(count: 2) // Should be executing twce
            .WithPackageName("mock-package");

        // -- Act --
        await testData.ActAsync(actOptions);

        // -- Assert --
        testData
            .HadNoException()
            .PackageIsInstalled()
            .VerifyValidationForInstalledChocoOccurred()
            .VerifyInstallationOfChocoOccurred()
            .VerifyExecutionOfChocoCommandOccurred(2);
    }

    [Fact]
    public async Task InstallPackageAsync_Throws_InvalidOperationException_From_InstallingChoco()
    {
        // -- Arrange --
        using var testData = new ChocolateyClientInstallPackageTestData(_logger)
            .WithValidateInstalledChoco()
                .ButNoInstalledChocoFound()
            .WithInstallChoco()
                .ButFailedToInstallChoco()
            .WithPackageName("mock-package");

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .HadException<InvalidOperationException>()
            .PackageIsNotInstalled()
            .VerifyValidationForInstalledChocoOccurred()
            .VerifyInstallationOfChocoOccurred()
            .VerifyExecutionOfChocoCommandOccurred(0);
    }

    [Fact]
    public async Task InstallPackageAsync_Fail_With_EmptyPackageName()
    {
        // -- Arrange --
        using var testData = new ChocolateyClientInstallPackageTestData(_logger)
            .WithPackageName(string.Empty);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .HadNoException()
            .PackageIsNotInstalled()
            .VerifyExecutionOfChocoCommandOccurred(0);
    }

    [Fact]
    public async Task InstallPackageAsync_Successfully()
    {
        // -- Arrange --
        using var testData = new ChocolateyClientInstallPackageTestData(_logger)
            .WithValidateInstalledChoco()
            .WithExecuteChocoCommand()
            .WithPackageName("mock-package");

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .HadNoException()
            .PackageIsInstalled()
            .VerifyValidationForInstalledChocoOccurred()
            .VerifyInstallationOfChocoOccurred(0)
            .VerifyExecutionOfChocoCommandOccurred();
    }
}
