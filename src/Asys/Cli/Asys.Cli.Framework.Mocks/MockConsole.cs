using System.CommandLine;
using System.CommandLine.IO;
using System.Text;
using Asys.Cli.Framework.Mocks.Internal;

namespace Asys.Cli.Framework.Mocks;

public class MockConsole : IConsole
{
    public override string ToString()
    {
        return ConsoleOutput;
    }

    private readonly StringBuilder _consoleOutput = new();

    public string ConsoleOutput => _consoleOutput.ToString();

    private readonly StringBuilder _stdOut = new StringBuilder();

    public string StandardOut => _stdOut.ToString();

    private readonly StringBuilder _stdErr = new StringBuilder();

    public string StandardError => _stdErr.ToString();

    public MockConsole(MockConsoleOptions? options = null)
    {
        Out = new StringBuilderStandardStreamWriter(_stdOut, _consoleOutput);
        Error = new StringBuilderStandardStreamWriter(_stdErr, _consoleOutput);

        options ??= new MockConsoleOptions();

        if (options.IsOutputRedirected)
        {
            Out = new StringBuilderStandardStreamWriter(_stdOut);
        }

        if (options.IsErrorRedirected)
        {
            Error = new StringBuilderStandardStreamWriter(_stdErr);
        }
    }

    public IStandardStreamWriter Out { get; }

    public bool IsOutputRedirected { get; set; }

    public IStandardStreamWriter Error { get; }

    public bool IsErrorRedirected { get; set; }

    public bool IsInputRedirected { get; set; }
}

