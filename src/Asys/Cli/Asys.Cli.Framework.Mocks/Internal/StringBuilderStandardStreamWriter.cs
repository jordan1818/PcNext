using System.CommandLine.IO;
using System.Text;

namespace Asys.Cli.Framework.Mocks.Internal;

public class StringBuilderStandardStreamWriter : IStandardStreamWriter
{
    private readonly StringBuilder[] _sbs;

    public StringBuilderStandardStreamWriter(params StringBuilder[] sbs)
    {
        _sbs = sbs;
    }

    public void Write(string? value)
    {
        foreach (var sb in _sbs)
        {
            sb.Append(value);
        }
    }
}
