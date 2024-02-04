using Asys.Tasks.Process.Context;
using Microsoft.Extensions.Logging;

namespace ChocolateySharp.Internal.Tasks.Contexts;

internal class ChocoProcessTaskContext : ProcessTaskContext
{
    public ChocoProcessTaskContext(ILogger logger) 
        : base(logger, "choco.exe")
    {
    }
}
