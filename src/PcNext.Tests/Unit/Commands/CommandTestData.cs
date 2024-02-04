using Asys.Tests.Framework;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine.Builder;
using PcNext.Cli;
using Asys.Cli.Framework.Mocks;
using Asys.Tests.Framework.Asserts;
using Asys.Tests.Framework.Logger;
using Microsoft.Extensions.Logging;

namespace PcNext.Tests.Unit.Commands;

internal abstract class CommandTestData<TCommandTestData> : ITestData
    where TCommandTestData : CommandTestData<TCommandTestData>
{
    private readonly ITestLogger _logger;

    protected CommandTestData(ITestLogger logger)
    {
        _logger = logger;
    }

    protected abstract string CommandArguments { get; }

    private int? ActualExitCode { get; set; }

    private string? ActualOutput { get; set; }

    private string? ActualErrorMessage { get; set; }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    public virtual void Dispose() { }

    public async Task ActAsync(ActOptions? actOptions = null)
    {
        // -- Arrange --
        var commandLine = new CommandLineBuilder()
            .AddApplication()
            .ConfigureServices((_, _, services) => ConfigureServices(services))
            .Build();

        // -- Act --
        var results = await commandLine.InvokeMockAsync(CommandArguments, new MockConsoleOptions { IsErrorRedirected = true, IsOutputRedirected = true });

        _logger.LogInformation(results.Console.ConsoleOutput);
        _logger.LogInformation(results.Console.StandardOut);
        _logger.LogInformation(results.Console.StandardError);

        ActualExitCode = results.ExitCode;
        ActualErrorMessage = results.Console.StandardError.Trim();
        ActualOutput = results.Console.StandardOut.Trim();
    }

    public TCommandTestData EnsureOutputIs(string expectedOutput)
    {
        Assert.Equal(expectedOutput, ActualOutput);
        return (TCommandTestData)this;
    }

    public TCommandTestData EnsureErrorMessageIs(string expectedErrorMessage)
    {
        Assert.Equal(expectedErrorMessage, ActualErrorMessage);
        return (TCommandTestData)this;
    }

    public TCommandTestData EnsureExitCodeIs(int expectedExitCode)
    {
        Assert.Equal(expectedExitCode, ActualExitCode);
        return (TCommandTestData)this;
    }

    public TCommandTestData EnsureExitCodeIsSuccess() => EnsureExitCodeIs(0);
}
