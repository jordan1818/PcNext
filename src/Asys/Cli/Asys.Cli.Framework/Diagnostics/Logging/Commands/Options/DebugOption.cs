using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace Asys.Cli.Framework.Diagnostics.Logging.Commands.Options;

/// <summary>
/// Defines a <see cref="Option"/> to specify the logging level as <see cref="LogLevel.Trace"/>.
/// </summary>
/// <remarks>
/// When added as an <see cref="Option"/>, it will be available as "--debug".
/// </remarks>
public class DebugOption : SwitchOption
{
    /// <summary>
    /// Initializes a new <see cref="DebugOption"/>.
    /// </summary>
    public DebugOption()
        : base(new[] { "-d", "--debug" })
    {
        // To avoid having too many options, we hide this one and
        // mention in the description of the "verbosity" option
        // an option of specifying "--debug" exists.
        IsHidden = true;
    }
}
