using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Asys.Cli.Framework.Internal;
using Asys.System.Environment;

namespace System.CommandLine.Builder;

/// <summary>
/// <see cref="IHost"/> extension methods for <see cref="CommandLineBuilder"/>.
/// </summary>
public static class HostingInternalCliBuilderExtensions
{
    /// <summary>
    /// Register a handler to configure the <see cref="IHost"/> services.
    /// </summary>
    /// <remarks>
    /// Equivalent to use <see cref="IHostBuilder.ConfigureServices(Action{HostBuilderContext, IServiceCollection})"/>.
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The setup to configure the <see cref="IHost"/> services.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureServices(this CommandLineBuilder builder, Action<InvocationContext, HostBuilderContext, IServiceCollection> setup)
    {
        return builder.ConfigureHosting((context, hostBuilder) => hostBuilder.ConfigureServices((host, services) => setup.Invoke(context, host, services)));
    }

    /// <summary>
    /// Register a handler to configure the <see cref="IHost"/> host configuration.
    /// </summary>
    /// <remarks>
    /// Equivalent to use <see cref="IHostBuilder.ConfigureHostConfiguration(Action{IConfigurationBuilder})"/>.
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The setup to configure the <see cref="IHost"/> host configuration.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureHostConfiguration(this CommandLineBuilder builder, Action<InvocationContext, IConfigurationBuilder> setup)
    {
        return builder.ConfigureHosting((context, hostBuilder) => hostBuilder.ConfigureHostConfiguration((configBuilder) => setup.Invoke(context, configBuilder)));
    }

    /// <summary>
    /// Register a handler to configure the <see cref="IHost"/> application configuration.
    /// </summary>
    /// <remarks>
    /// Equivalent to use <see cref="IHostBuilder.ConfigureAppConfiguration(Action{HostBuilderContext, IConfigurationBuilder})"/>.
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The setup to configure the <see cref="IHost"/> application configuration.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureAppConfiguration(this CommandLineBuilder builder, Action<InvocationContext, HostBuilderContext, IConfigurationBuilder> setup)
    {
        return builder.ConfigureHosting((context, hostBuilder) => hostBuilder.ConfigureAppConfiguration((host, configBuilder) => setup.Invoke(context, host, configBuilder)));
    }

    /// <summary>
    /// Register a handler that will be called when building the <see cref="IHost"/> via <see cref="IHostBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The setup to call when building the <see cref="IHost"/> via <see cref="IHostBuilder"/>.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureHosting(this CommandLineBuilder builder, Action<InvocationContext, IHostBuilder> setup)
    {
        // Add the setup in the relevant build time property
        // which will be called by the host handling logic.
        var hostConfigurationHandlers = builder.Command.GetBuildTimeProperty(nameof(ConfigureHosting), factory: () => new List<Action<InvocationContext, IHostBuilder>>());
        hostConfigurationHandlers.Add(setup);

        // Ensures the host handling is registered.
        builder.AddHostingHandling();

        return builder;
    }

    /// <summary>
    /// Register a handler that will be called after the <see cref="IHost"/> is started.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The setup to call when the <see cref="IHost"/> is started.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureOnHostStarted(this CommandLineBuilder builder, Action<InvocationContext, IHost> setup)
    {
        // Add the setup in the relevant build time property
        // which will be called by the host handling logic.
        var hostStartedHandlers = builder.Command.GetBuildTimeProperty(nameof(ConfigureOnHostStarted), factory: () => new List<Action<InvocationContext, IHost>>());
        hostStartedHandlers.Add(setup);

        // Ensures the host handling is registered.
        builder.AddHostingHandling();

        return builder;
    }

    public static CommandLineBuilder UseHostBuilder(this CommandLineBuilder builder, Func<string[], IHostBuilder> hostBuilderFactory)
    {
        builder.Command.SetBuildTimeProperty(nameof(UseHostBuilder), hostBuilderFactory);

        // Ensures the host handling is registered.
        builder.AddHostingHandling();

        return builder;
    }

    private static CommandLineBuilder AddHostingHandling(this CommandLineBuilder builder)
    {
        // To avoid mis-behavior, we call this step only once since
        // it registers the IHost and we also add a "system" middleware.
        return builder.ExecuteOnlyOnce(nameof(AddHostingHandling), () =>
        {
            // Use the IHost as defined in System.CommandLine.Hosting and does some
            // minor configuration to ensure it complies with what we expect it.
            // We should be able to specify the host builder factory and be able to register
            // a custom host builder if needed.
            builder.UseHost((s) =>
            {
                var hostBuilderFactory = builder.Command.GetBuildTimeProperty<Func<string[], IHostBuilder>>(nameof(UseHostBuilder), () => (s) => new HostBuilder());
                return hostBuilderFactory.Invoke(s);

            }, hostBuilder =>
            {
                var context = hostBuilder.GetInvocationContext();

                // Remove the default config source which parses the command line arguments.
                hostBuilder.ConfigureHostConfiguration(x => x.Sources.Clear());

                // Register the IEnvironmentVariables as it is a "must have" service for the CLI.
                var environmentVariables = context.BindingContext.GetService<IEnvironmentVariables>() ?? new EnvironmentVariables();
                hostBuilder.Properties[typeof(IEnvironmentVariables)] = environmentVariables;
                hostBuilder.ConfigureServices(services =>
                {
                    services.TryAddSingleton(environmentVariables);
                });

                // Call all the setup handlers that have been registered
                // to configure the host, including services, configuration, etc...
                var registeredHandlers = context.ParseResult.RootCommandResult.Command.GetBuildTimeProperty(nameof(ConfigureHosting), () => new List<Action<InvocationContext, IHostBuilder>>());
                foreach (var handler in registeredHandlers)
                {
                    handler.Invoke(context, hostBuilder);
                }
            });

            // Add a "system" middleware calling all the
            // setup registered for when the host is started.
            builder.AddMiddleware(async (context, next) =>
            {
                var host = context.BindingContext.GetRequiredService<IHost>();
                var registeredHandlers = context.ParseResult.RootCommandResult.Command.GetBuildTimeProperty(nameof(ConfigureOnHostStarted), () => new List<Action<InvocationContext, IHost>>());

                foreach (var handler in registeredHandlers)
                {
                    handler.Invoke(context, host);
                }

                await next(context).ConfigureAwait(false);
            });
        });
    }
}
