using System.CommandLine;
using Microsoft.Extensions.Logging;

namespace Asys.Cli.Framework.Diagnostics.Logging.Commands.Options;

/// <summary>
/// Defines a <see cref="Option"/> to specify the logging level.
/// </summary>
/// <remarks>
/// When added as an <see cref="Option"/>, it will be available as "--verbosity".
/// </remarks>
public class LogLevelOption : Option<LogLevel?>
{
    /// <summary>
    /// Initializes a new <see cref="LogLevelOption"/>.
    /// </summary>
    public LogLevelOption()
        : base(new[] { "-v", "--verbosity" }, "The tracing output level. You can also specify '-d' ('--debug') to set level as 'Trace' or '-q' ('--quiet') to set the level as 'None'. Setting up the environment variable 'DEBUG' has the same effect as specifying '-d' in your command if you did not specify other values.")
    {
#pragma warning disable MA0056 // Do not call overridable members in constructor
        Arity = ArgumentArity.ExactlyOne;
#pragma warning restore MA0056 // Do not call overridable members in constructor
    }
}
