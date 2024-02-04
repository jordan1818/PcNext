using System.CommandLine.IO;
using System.Text;

namespace Asys.Cli.Framework.Mocks.Internal;

public class StandardStreamTextWriter : TextWriter
{
    private readonly IStandardStreamWriter _ssw;

    public StandardStreamTextWriter(IStandardStreamWriter ssw)
    {
        _ssw = ssw;
    }

    public override void Write(char value)
    {
        _ssw.Write(value.ToString());
    }

    public override Encoding Encoding => Encoding.Default;

}
