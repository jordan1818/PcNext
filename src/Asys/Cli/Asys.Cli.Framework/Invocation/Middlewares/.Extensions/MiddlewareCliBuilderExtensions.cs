using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Asys.Cli.Framework.Invocation.Middlewares;
using Asys.Cli.Framework.Invocation.Middlewares.Internal;

namespace System.CommandLine.Builder;

public static class MiddlewareCliBuilderExtensions
{
    /// <summary>
    /// Add a <see cref="IMiddleware"/> to the CLI.
    /// </summary>
    /// <remarks>
    /// Internally, it also takes care of calling <see cref="AddMiddlewareHandling(CommandLineBuilder)"/>.
    /// </remarks>
    /// <typeparam name="TMiddleware">The <see cref="IMiddleware"/> type.</typeparam>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder AddMiddleware<TMiddleware>(this CommandLineBuilder builder)
        where TMiddleware : class, IMiddleware
    {
        builder.ConfigureHosting((context, hostBuilder) => hostBuilder.ConfigureServices(services => services.AddMiddleware<TMiddleware>()));

        builder.AddMiddlewareHandling();

        return builder;
    }

    /// <summary>
    /// Add the handling for the registered <see cref="IMiddleware"/>.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder AddMiddlewareHandling(this CommandLineBuilder builder)
    {
        // Since the handling is registering a "system"
        // middleware, then we want to add it only once
        builder.ExecuteOnlyOnce(nameof(AddMiddlewareHandling), () =>
        {
            // Adds the necessary services in the service collection.
            builder.ConfigureHosting((context, hostBuilder) => hostBuilder.ConfigureServices(services => services.AddMiddlewareHandling()));

            // Adds the "system" middleware to retrieve the registered middlewares
            // and call them as a chain-of-responsibility pattern.
            builder.AddMiddleware(async (context, next) =>
            {
                var host = context.BindingContext.GetRequiredService<IHost>();
                var middlewareHandler = host.Services.GetRequiredService<MiddlewareHandler>();

                await middlewareHandler.InvokeAsync(context, next).ConfigureAwait(false);
            });
        });

        return builder;
    }
}
