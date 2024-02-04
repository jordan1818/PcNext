using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Framework.Tasks;

public sealed class RegistryTaskTests
{
    private readonly ITestLogger _logger;

    public RegistryTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }

    [Fact]
    public async Task ExecuteAsync_Failure_NotOnWindows()
    {
        // -- Arrange --
        using var testData = new RegistryTaskTestData(_logger)
            .WithOsIsNotWindows();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyNoRegistryInteractions();
    }

    [Theory]
    [InlineData("")] // Empty value
    [InlineData("invalid-value")] // Invalid value
    public async Task ExecuteAsync_Failure_InvalidParameter_Hive(string value)
    {
        // -- Arrange --
        using var testData = new RegistryTaskTestData(_logger)
            .WithHive(value);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyNoRegistryInteractions();
    }

    [Theory]
    [InlineData("")] // Empty value
    [InlineData("invalid-value")] // Invalid value
    public async Task ExecuteAsync_Failure_InvalidParameter_Action(string value)
    {
        // -- Arrange --
        using var testData = new RegistryTaskTestData(_logger)
            .WithAction(value);

        // -- Act --
         await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyNoRegistryInteractions();
    }

    [Theory]
    [InlineData("")] // Empty value
    public async Task ExecuteAsync_Failure_InvalidParameter_Path(string value)
    {
        // -- Arrange --
        using var testData = new RegistryTaskTestData(_logger)
            .WithPath(value);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyNoRegistryInteractions();
    }

    [Theory]
    [InlineData("")] // Empty value
    public async Task ExecuteAsync_Failure_InvalidParameter_Key(string value)
    {
        // -- Arrange --
        using var testData = new RegistryTaskTestData(_logger)
            .WithKey(value);

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsExceptionEmpty()
            .IsTaskExceptionEmpty()
            .VerifyNoRegistryInteractions();
    }

    [Fact]
    public async Task ExecuteAsync_Failure_RegistryKey_Throws()
    {
        // -- Arrange --
        using var testData = new RegistryTaskTestData(_logger)
            .ThrowOnRegistryHiveRetrieval<Exception>();

        // -- Act --
        await testData.ActAsync();

        // -- Assert --
        testData
            .IsResultsFailureMessageNotEmpty()
            .IsResultsFailure()
            .IsResultsWithException<Exception>()
            .IsTaskExceptionEmpty()
            .VerifyNoRegistryUpdates();
    }
}
