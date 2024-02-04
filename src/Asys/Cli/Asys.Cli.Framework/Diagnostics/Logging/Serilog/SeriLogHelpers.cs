using System.CommandLine.Invocation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Asys.Cli.Framework.Diagnostics.Logging.Serilog.Enrichers;

namespace Asys.Cli.Framework.Diagnostics.Logging.Serilog;

/// <summary>
/// Defines helpers to use with Serilog.
/// </summary>
public static class SeriLogHelpers
{
    /// <summary>
    /// When this property is added to the logs, then it should not be logged in the console.
    /// </summary>
    public const string NoConsoleLoggingProperty = "NoConsoleLogging";


    /// <summary>
    /// Uses the default serilog configuration.
    /// </summary>
    /// <remarks>
    /// It enables the following:
    /// <list type="bullet">
    /// <item>
    ///     <term><code>.Enrich.FromLogContext()</code></term>
    ///     <description>Enrich log properties with the logging scope info.</description>
    /// </item>
    /// <item>
    ///     <term><code>.Enrich.With<ExceptionTypeEnricher>()</code></term>
    ///     <description>Enrich log properties with the <see cref="Exception"/> info.</description>
    /// </item>
    /// <item>
    ///     <term><code>.Enrich.WithSensitiveDataMasking()</code></term>
    ///     <description>Masks sensitive information in logging messages.</description>
    /// </item>
    /// <item>
    ///     <term><code>.MinimumLevel.Verbose()</code></term>
    ///     <description>Sets the minimum level of logging to Verbose. This can then be overriden by specific logger implementation.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="context">The <see cref="InvocationContext"/> instance.</param>
    /// <param name="host">The <see cref="HostBuilderContext"/> instance.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> instance.</param>
    /// <param name="serilog">The <see cref="LoggerConfiguration"/> instance.</param>
    public static void UseDefault(InvocationContext context, HostBuilderContext host, IServiceProvider services, LoggerConfiguration serilog)
    {
        serilog
            .Enrich.FromLogContext()
            .Enrich.With<ExceptionTypeEnricher>()
            .MinimumLevel.Verbose();
    }

    /// <summary>
    /// Logs to the console.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> instance.</param>
    /// <param name="host">The <see cref="HostBuilderContext"/> instance.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> instance.</param>
    /// <param name="serilog">The <see cref="LoggerConfiguration"/> instance.</param>
    public static void UseConsoleLogging(InvocationContext context, HostBuilderContext host, IServiceProvider services, LoggerConfiguration serilog)
    {
        var options = services.GetService<IOptions<SerilogConsoleConfiguration>>()?.Value ?? new SerilogConsoleConfiguration();

        if (options.Enabled)
        {
            serilog.WriteTo.Logger(l => l
                .Filter.ByExcluding(e => e.Properties.Keys.Any(p => p?.Equals(NoConsoleLoggingProperty, StringComparison.OrdinalIgnoreCase) == true))
                .WriteTo.Console(
                    options.RestrictedToMinimumLevel,
                    options.OutputTemplate,
                    options.FormatProvider,
                    options.LevelSwitch,
                    options.StandardErrorFromLevel,
                    options.Theme,
                    options.ApplyThemeToRedirectedOutput
                ));
        }
    }

    /// <summary>
    /// Logs to a file in AppData folder.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> instance.</param>
    /// <param name="host">The <see cref="HostBuilderContext"/> instance.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> instance.</param>
    /// <param name="serilog">The <see cref="LoggerConfiguration"/> instance.</param>
    /// <param name="relativeDirectoryPath">The folder path where the log file will be output.</param>
    public static void UseAppDataLogging(InvocationContext context, HostBuilderContext host, IServiceProvider services, LoggerConfiguration serilog, string? relativeDirectoryPath = "logs")
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var logFilePath = Path.Combine(appDataPath, host.HostingEnvironment.ApplicationName);
        if (!string.IsNullOrWhiteSpace(relativeDirectoryPath))
        {
            logFilePath = Path.Combine(logFilePath, relativeDirectoryPath);
        }

        logFilePath = Path.Combine(logFilePath, $"log-{Guid.NewGuid()}.log");
        serilog.WriteTo.File(logFilePath, restrictedToMinimumLevel: LogEventLevel.Verbose);
    }

    /// <summary>
    /// Logs to a file in ContentRootPath folder.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> instance.</param>
    /// <param name="host">The <see cref="HostBuilderContext"/> instance.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> instance.</param>
    /// <param name="serilog">The <see cref="LoggerConfiguration"/> instance.</param>
    /// <param name="relativeDirectoryPath">The folder path where the log file will be output.</param>
    public static void UseContentRootPathFileLogging(InvocationContext context, HostBuilderContext host, IServiceProvider services, LoggerConfiguration serilog, string? relativeDirectoryPath = "logs")
    {
        var logFilePath = host.HostingEnvironment.ContentRootPath;
        if (!string.IsNullOrWhiteSpace(relativeDirectoryPath))
        {
            logFilePath = Path.Combine(logFilePath, relativeDirectoryPath);
        }

        logFilePath = Path.Combine(logFilePath, $"log-{Guid.NewGuid()}.log");
        serilog.WriteTo.File(logFilePath, restrictedToMinimumLevel: LogEventLevel.Verbose);
    }

    /// <summary>
    /// Logs to a file in working directory.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> instance.</param>
    /// <param name="host">The <see cref="HostBuilderContext"/> instance.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> instance.</param>
    /// <param name="serilog">The <see cref="LoggerConfiguration"/> instance.</param>
    /// <param name="relativeDirectoryPath">The folder path where the log file will be output.</param>
    public static void UseWorkingDirectoryFileLogging(InvocationContext context, HostBuilderContext host, IServiceProvider services, LoggerConfiguration serilog, string? relativeDirectoryPath = "logs")
    {
        var logFilePath = Path.Combine(Environment.CurrentDirectory, host.HostingEnvironment.ApplicationName);
        if (!string.IsNullOrWhiteSpace(relativeDirectoryPath))
        {
            logFilePath = Path.Combine(logFilePath, relativeDirectoryPath);
        }

        logFilePath = Path.Combine(logFilePath, $"log-{Guid.NewGuid()}.log");
        serilog.WriteTo.File(logFilePath, restrictedToMinimumLevel: LogEventLevel.Verbose);
    }
}
