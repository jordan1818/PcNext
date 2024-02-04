using Microsoft.Extensions.Logging;

namespace Asys.Tasks.Process.Context;

/// <summary>
/// A defintion of <see cref="CmdProcessTaskContext"/> based off of <see cref="ProcessTaskContext"/>
/// which contains additional CMD context.
/// </summary>
/// <remarks>
/// See https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/cmd
/// for more information about the command line arguments.
/// </remarks>
public class CmdProcessTaskContext : ProcessTaskContext
{
    /// <summary>
    /// Initializes an instance of <see cref="CmdProcessTaskContext"/>.
    /// </summary>
    /// <param name="logger">The logger to be used for the <see cref="CmdProcessTask"/> or <see cref="CmdProcessTask{TCmdProcessTaskContext, TCmdProcessTaskResults}"/>.</param>
    public CmdProcessTaskContext(ILogger logger)
        : base(logger, "cmd.exe")
    {
    }

    /// <inheritdoc/>
    public override string? FullArugments
    {
        get 
        {
            var cmdArguments = new List<string>();

            if (ExecuteThenTerminate)
            {
                cmdArguments.Add("/c");
            }
            
            return $"{string.Join(" ", cmdArguments)} {Arguments}";
        }
    }

    /// <summary>
    /// This carries out the command specified and then exits the command.
    /// By default, it is true.
    /// </summary>
    public bool ExecuteThenTerminate { get; set; } = true;
}
