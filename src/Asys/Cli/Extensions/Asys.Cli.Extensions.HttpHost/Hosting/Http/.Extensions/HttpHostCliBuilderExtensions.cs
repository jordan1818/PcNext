using System.CommandLine.Invocation;
using System.Reflection;
using Asys.Cli.Framework.Internal;

namespace System.CommandLine.Builder;

/// <summary>
/// <see cref="IWebHost"/> extension methods for <see cref="CommandLineBuilder"/>.
/// </summary>
public static class WebHostCliBuilderExtensions
{
    /// <summary>
    /// Uses the default <see cref="IWebHost"/> configuration with MVC support.
    /// </summary>
    /// <remarks>
    /// It registers the default MVC and Swagger services alongside the Swagger and MVC HTTP pipeline handlers.
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="urls">The urls the hosted application will listen on.</param>
    /// <param name="setup">The extra setup to configure the <see cref="IWebHostBuilder"/> further.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseDefaultMvcWebHost(this CommandLineBuilder builder, string urls, Action<InvocationContext, IHostBuilder, IWebHostBuilder>? setup = null)
    {
        builder.ConfigureWebHostDefaults((context, host, web) =>
        {
            web
                .UseUrls(urls)
                .AddDefaultMvcServices()
                .AddDefaultSwaggerServices()
                .Configure(app =>
                {
                    // Configure is not cumulative, instead, it replaces everytime
                    // it is called. Setting it here implies the developer need to do their
                    // own implementation if needed.
                    app
                        .UseDefaultSwagger()
                        .UseDefaultMvc();
                });

            // Invokes the extra setup specified.
            setup?.Invoke(context, host, web);
        });


        return builder;
    }

    /// <summary>
    /// Configures the default <see cref="IWebHost"/> started alongside the command execution.
    /// </summary>
    /// <remarks>
    /// The HTTP host will be started only if the <see cref="ConfigureWebHost(CommandLineBuilder, Func{InvocationContext, bool}, Action{InvocationContext, IHostBuilder, IWebHostBuilder})"/> has
    /// been invoked as well and the predicate specified the <see cref="IWebHost"/> should indeed be started.
    /// does not return.
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The setup to configure the <see cref="IWebHostBuilder"/>.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureWebHostDefaults(this CommandLineBuilder builder, Action<InvocationContext, IHostBuilder, IWebHostBuilder> setup)
    {
        // we register the default setups under a build-time property that will be resolved
        // only if an executing command uses an HTTP host. Else it will be discarded.
        var defaultSetups = builder.Command.GetBuildTimeProperty(nameof(ConfigureWebHostDefaults), () => new List<Action<InvocationContext, IHostBuilder, IWebHostBuilder>>());
        defaultSetups.Add(setup);

        return builder;
    }

    /// <summary>
    /// Configures the <see cref="IWebHost"/> started alongside the command execution.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="predicate">Defines if the <see cref="IWebHost"/> should be started alongside the executing command.</param>
    /// <param name="setup">The setup to configure the <see cref="IWebHost"/>.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureWebHost(this CommandLineBuilder builder, Func<InvocationContext, bool> predicate, Action<InvocationContext, IHostBuilder, IWebHostBuilder> setup)
    {
        // We register the setup and its associated predicate in a specific collection
        // that will be resolved at runtime when the invocation context is available.
        var specificSetups = builder.Command.GetBuildTimeProperty(nameof(ConfigureWebHost), () => new List<(Func<InvocationContext, bool> predicate, Action<InvocationContext, IHostBuilder, IWebHostBuilder> setup)>());
        specificSetups.Add((predicate, setup));

        builder.ExecuteOnlyOnce(nameof(ConfigureWebHost), () =>
        {
            builder.ConfigureHosting((context, host) =>
            {
                // we retrieve the list of specific setup and as soon as
                // we encounter one that requires the HTTP host to be started then we
                // continue with the setup, else we do not register the HTTP host.
                var specificSetups = builder.Command.GetRequiredBuildTimeProperty<List<(Func<InvocationContext, bool> predicate, Action<InvocationContext, IHostBuilder, IWebHostBuilder> setup)>>(nameof(ConfigureWebHost));
                for(var i =0; i< specificSetups.Count;i++)
                {
                    var specificSetup = specificSetups[i];

                    // Ensures to start the HTTP host only when it has been
                    // mentioned explicitely the HTTP host should be started.
                    if (specificSetup.predicate.Invoke(context))
                    {
                        // ConfigureWebHostDefaults is not cumulative so we must call it only once.
                        host.ConfigureWebHostDefaults(web =>
                        {
                            // If default setups have been registered, then we invoke them
                            // before any specific setup. We then clear the collection to
                            // ensure they are not called twice.
                            var defaultSetups = builder.Command.GetBuildTimeProperty(nameof(ConfigureWebHostDefaults), () => new List<Action<InvocationContext, IHostBuilder, IWebHostBuilder>>());
                            foreach (var defaultSetup in defaultSetups)
                            {
                                defaultSetup.Invoke(context, host, web);
                            }
                            defaultSetups.Clear();

                            // We then invoke the current setup and continue throughout the
                            // collection of setups to add any other specifics that needs to be added.
                            specificSetup.setup.Invoke(context, host, web);

                            for (var j = i + 1; j < specificSetups.Count; j++)
                            {
                                var nextSpecificSetup = specificSetups[j];
                                if (nextSpecificSetup.predicate.Invoke(context))
                                {
                                    // If the next specific setup needs to be enabled, then
                                    // we invoke its setup as well.
                                    nextSpecificSetup.setup.Invoke(context, host, web);
                                }
                            }

                            // In order to ensure the entry assembly is used as "application"
                            // by the web host, we need to override the app key setting, else it will
                            // use the executing assembly which is this one.
                            var entryAssembly = Assembly.GetEntryAssembly();
                            if (entryAssembly != null)
                            {
                                web.UseSetting(WebHostDefaults.ApplicationKey, entryAssembly.GetName().Name);
                            }
                        });

                        // As soon as one setup has been identified as relevant, then
                        // we exit since calling multiple times ConfigureWebHostDefaults
                        // results in misbehavior.
                        break;
                    }
                }
            });
        });

        return builder;
    }

    /// <summary>
    /// Configures when the <see cref="IWebHost"/> should be started.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="predicate">Defines when the <see cref="IWebHost"/> should be started alongside the executing command.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder EnableWebHost(this CommandLineBuilder builder, Func<InvocationContext, bool> predicate)
    {
        return builder.ConfigureWebHost(predicate, (_, _, _) => { });
    }
}
