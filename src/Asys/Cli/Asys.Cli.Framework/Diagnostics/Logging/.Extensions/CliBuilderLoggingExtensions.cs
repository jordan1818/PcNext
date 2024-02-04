using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Asys.Cli.Framework.Diagnostics.Logging.Commands.Options;
using Asys.Cli.Framework.Diagnostics.Logging.Serilog;
using Asys.Cli.Framework.Internal;

namespace Asys.Cli.Framework.Diagnostics.Logging;

/// <summary>
/// Logging extension methods for <see cref="CommandLineBuilder"/>.
/// </summary>
public static class CliBuilderLoggingExtensions
{
    /// <summary>
    /// Enables the default CLI framework logging behaviors for <paramref name="builder"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should be called only once else it might result in mis-behavior.
    /// </para>
    /// It enables the following:
    /// <list type="bullet">
    /// <item>
    ///     <term><code>.UseConsoleLogging()</code></term>
    ///     <description>Logs to the console stderr.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseLogLevelCommandOption()</code></term>
    ///     <description>Enables <see cref="LogLevel"/> to be specified in the command line.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseAppDataLogging()</code></term>
    ///     <description>Logs to files in AppData folders.</description>
    /// </item>
    /// </list>
    /// <para>
    /// For more information about <see cref="LogLevel"/>, see <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-7.0#log-level"/>.
    /// </para>
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseDefaultLogging(this CommandLineBuilder builder)
    {
        return builder
            .UseDefaultConsoleLogging()
            .UseLogLevelCommandOption()
            .UseAppDataLogging();
    }

    /// <summary>
    /// Enables <see cref="LogLevel"/> to be specified in the command line.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseLogLevelCommandOption(this CommandLineBuilder builder)
    {
        // Since we add options, we want to make sure this step is called only once.
        builder.ExecuteOnlyOnce(nameof(UseLogLevelCommandOption), () =>
        {
            // We add the required options and save them as
            // service since they will be used via DI by
            // subsequent services.
            var logLevelOption = new LogLevelOption();
            builder.Command.AddGlobalOption(logLevelOption);

            var debugOption = new DebugOption();
            builder.Command.AddGlobalOption(debugOption);

            var quietOption = new QuietOption();
            builder.Command.AddGlobalOption(quietOption);

            builder.ConfigureServices((context, host, services) =>
            {
                services.AddSingleton(logLevelOption);
                services.AddSingleton(debugOption);
                services.AddSingleton(quietOption);
            });

            // We want to configure the verbosity only for the console
            // since we want to have as much detail as possible for the
            // file logging.
            // It would be interesting to wrap this logic in a central
            // service so we can easily "ask" for the selected loglevel in other contexts.
            builder.UseConsoleLogging((context, host, options) =>
            {
                if (context.ParseResult.GetValueForOption(debugOption))
                {
                    // If --debug has been specified, then we enable the lowest level possible
                    options.RestrictedToMinimumLevel = LogEventLevel.Verbose;
                }
                else if (context.ParseResult.GetValueForOption(quietOption))
                {
                    // If --quiet has been specified, then we disable the console logging
                    options.Enabled = false;
                }
                else
                {
                    // Else we parse the verbosity option value and determine the log level
                    // that will be used for the console.
                    var logLevel = context.ParseResult.GetValueForOption(logLevelOption);

                    // We also support the DEBUG environment variable which is equivalent to "--debug".
                    // This could be bound to the debug option directly.
                    var debugEnv = host.Configuration.GetValue<string?>("DEBUG");
#pragma warning disable MA0073 // Avoid comparison with bool constant
                    if (logLevel == null && (debugEnv != null && (!bool.TryParse(debugEnv, out var b) || b != false)))
                    {
                        options.RestrictedToMinimumLevel = LogEventLevel.Verbose;
                    }
                    else
                    {
                        logLevel ??= LogLevel.Warning;

                        switch (logLevel)
                        {
                            case LogLevel.Trace:
                                options.RestrictedToMinimumLevel = LogEventLevel.Verbose;
                                break;
                            case LogLevel.Debug:
                                options.RestrictedToMinimumLevel = LogEventLevel.Debug;
                                break;
                            case LogLevel.Information:
                                options.RestrictedToMinimumLevel = LogEventLevel.Information;
                                break;
                            case LogLevel.Warning:
                                options.RestrictedToMinimumLevel = LogEventLevel.Warning;
                                break;
                            case LogLevel.Error:
                                options.RestrictedToMinimumLevel = LogEventLevel.Error;
                                break;
                            case LogLevel.Critical:
                                options.RestrictedToMinimumLevel = LogEventLevel.Fatal;
                                break;
                            case LogLevel.None:
                                options.Enabled = false;
                                break;
                        }
                    }
#pragma warning restore MA0073 // Avoid comparison with bool constant
                }
            });
        });

        return builder;
    }

    /// <summary>
    /// Enables logging to the console with default behavior when output is redirected.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseDefaultConsoleLogging(this CommandLineBuilder builder)
    {
        return builder
            .UseConsoleLogging((context, host, options) =>
            {
                if (!context.Console.IsErrorRedirected && context.Console.IsOutputRedirected)
                {
                    options.ApplyThemeToRedirectedOutput = true;
                }
            });
    }

    /// <summary>
    /// Enables logging to the console.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The console configuration handler.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseConsoleLogging(this CommandLineBuilder builder, Action<InvocationContext, HostBuilderContext, SerilogConsoleConfiguration>? setup = null)
    {
        if (setup != null)
        {
            builder.ConfigureServices((context, host, services) => services.Configure<SerilogConsoleConfiguration>(o => setup.Invoke(context, host, o)));
        }

        // To avoid having multiple console logs, we execute only once.
        return builder.ExecuteOnlyOnce(nameof(UseConsoleLogging), () => builder.ConfigureLogging((context, host, services, serilog) => SeriLogHelpers.UseConsoleLogging(context, host, services, serilog)));
    }

    /// <summary>
    /// Enables logging to a file in AppData folder.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="relativeDirectoryPath">The folder path where the log file will be output.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAppDataLogging(this CommandLineBuilder builder, string? relativeDirectoryPath = "logs")
    {
        return builder.ConfigureLogging((context, host, services, serilog) => SeriLogHelpers.UseAppDataLogging(context, host, services, serilog, relativeDirectoryPath));
    }

    /// <summary>
    /// Enables logging to a file in ContentRootPath folder.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="relativeDirectoryPath">The folder path where the log file will be output.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseContentRootPathFileLogging(this CommandLineBuilder builder, string? relativeDirectoryPath = "logs")
    {
        return builder.ConfigureLogging((context, host, services, serilog) => SeriLogHelpers.UseContentRootPathFileLogging(context, host, services, serilog, relativeDirectoryPath));
    }

    /// <summary>
    /// Enables logging to a file in working directory folder.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="relativeDirectoryPath">The folder path where the log file will be output.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseWorkingDirectoryFileLogging(this CommandLineBuilder builder, string? relativeDirectoryPath = "logs")
    {
        return builder.ConfigureLogging((context, host, services, serilog) => SeriLogHelpers.UseWorkingDirectoryFileLogging(context, host, services, serilog, relativeDirectoryPath));
    }

    /// <summary>
    /// Allows to configure logging.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The logging configuration handler.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureLogging(this CommandLineBuilder builder, Action<InvocationContext, HostBuilderContext, IServiceProvider, LoggerConfiguration> setup)
    {
        var buildTimePropertKey = nameof(ConfigureLogging);

        builder.ExecuteOnlyOnce(nameof(ConfigureLogging), () =>
        {
            builder.ConfigureHosting((context, hostBuilder) =>
            {
                hostBuilder.UseSerilog((host, services, serilog) =>
                {
                    SeriLogHelpers.UseDefault(context, host, services, serilog);

                    var registeredHandlers = context.ParseResult.RootCommandResult.Command.GetRequiredBuildTimeProperty<List<Action<InvocationContext, HostBuilderContext, IServiceProvider, LoggerConfiguration>>>(buildTimePropertKey);
                    foreach (var handler in registeredHandlers)
                    {
                        handler.Invoke(context, host, services, serilog);
                    }
                });
            });
        });

        var loggingConfigurationHandlers = builder.Command.GetBuildTimeProperty(buildTimePropertKey, factory: () => new List<Action<InvocationContext, HostBuilderContext, IServiceProvider, LoggerConfiguration>>());
        loggingConfigurationHandlers.Add(setup);

        return builder;
    }
}
