namespace Asys.Cli.Framework.Mocks.Invocation;

public class MockInvocationResult
{
    public int ExitCode { get; }

    public MockConsole Console { get; }

    public MockInvocationResult(int exitCode, MockConsole console)
    {
        ExitCode = exitCode;
        Console = console;
    }
}
