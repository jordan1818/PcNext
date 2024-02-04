using Asys.System.Environment;
using Asys.System.Environment.Windows.Registry;
using Asys.System.Environment.Windows.Scheduler;
using Asys.System.IO;
using Asys.System.Security;
using Asys.Tasks.Process;
using Asys.Cli.Extensions.Asys.System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace System.CommandLine.Builder;

/// <summary>
/// Enables the default CLI Asys System behaviors for <paramref name="builder"/>.
/// </summary>
public static class AsysCliAsysExtensions
{
    /// <summary>
    /// Enables Asys default services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAsysDefaults(this CommandLineBuilder builder) =>
        builder.UseAsysSystemDefaults()
            .UseAsysTasksServices();

    /// <summary>
    /// Enables Asys System default services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAsysSystemDefaults(this CommandLineBuilder builder) => 
        builder
            .ConfigureServices((context, hostBuilderContext, services) =>
            {
                services.TryAddSingleton<IEnvironmentVariables, EnvironmentVariables>();
                services.TryAddSingleton<IAccountManager, AccountManager>();
                services.TryAddSingleton<IOperatingSystem, Asys.System.Environment.OperatingSystem>();
            })
            .UseAsysSystemIoDefaults()
            .UseAsysSystemWindowsDefaults();

    /// <summary>
    /// Enables Asys System IO default services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAsysSystemIoDefaults(this CommandLineBuilder builder) =>
        builder
        .ConfigureServices((context, hostBuilderContext, services) =>
        {
            services.TryAddSingleton<IFileSystemFactory, FileSystemFactory>();
            services.TryAddSingleton<IFileSystem, FileSystem>();
        });

    /// <summary>
    /// Enables Asys System Windows default services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAsysSystemWindowsDefaults(this CommandLineBuilder builder) =>
        builder
        .ConfigureServices((context, hostBuilderContext, services) =>
        {
            services.TryAddSingleton<IRegistry, Registry>();
            services.TryAddSingleton<ITaskScheduler, Asys.System.Environment.Windows.Scheduler.TaskScheduler>();
        });

    /// <summary>
    /// Enables Asys Tasks services for <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAsysTasksServices(this CommandLineBuilder builder) =>
        builder
        .ConfigureServices((context, hostBuilderContext, services) =>
        {
            services.TryAddSingleton<IProcessTaskFactory, ProcessTaskFactory>();
        });
}