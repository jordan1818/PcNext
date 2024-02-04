using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Asys.Cli.Framework.Diagnostics.Logging.Serilog;

/// <summary>
/// Defines options for the console logging.
/// </summary>
public class SerilogConsoleConfiguration
{
    /// <summary>
    /// Defines if logging to the console is enabled or not.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Defines the lowest logging level.
    /// </summary>
    public LogEventLevel RestrictedToMinimumLevel { get; set; } = LogEventLevel.Information;

    /// <summary>
    /// Defines the logging output message template.
    /// </summary>
    public string OutputTemplate { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// The logging message format provider.
    /// </summary>
    public IFormatProvider? FormatProvider { get; set; }

    /// <summary>
    /// The logging level switch.
    /// </summary>
    public LoggingLevelSwitch? LevelSwitch { get; set; }

    /// <summary>
    /// Defines at which level logging goes to stderr instead of stdout.
    /// </summary>
    public LogEventLevel? StandardErrorFromLevel { get; set; } = LogEventLevel.Verbose;

    /// <summary>
    /// Defiunes the console theming used with ANSI markers.
    /// </summary>
    public ConsoleTheme? Theme { get; set; } = AnsiConsoleTheme.Code;

    /// <summary>
    /// Defines if the theme should be used even when the output is redirected.
    /// </summary>
    public bool ApplyThemeToRedirectedOutput { get; set; }
}
