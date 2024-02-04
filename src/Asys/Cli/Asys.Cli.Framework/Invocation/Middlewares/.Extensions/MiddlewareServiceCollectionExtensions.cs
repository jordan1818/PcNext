using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Asys.Cli.Framework.Invocation.Middlewares;
using Asys.Cli.Framework.Invocation.Middlewares.Internal;

namespace System.CommandLine.Builder;

public static class MiddlewareServiceCollectionExtensions
{
    /// <summary>
    /// Add a <see cref="IMiddleware"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TMiddleware">The <see cref="IMiddleware"/> type.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection" /> instance.</param>
    /// <returns>The <paramref name="services"/> used for chaining.</returns>
    public static IServiceCollection AddMiddleware<TMiddleware>(this IServiceCollection services)
        where TMiddleware : class, IMiddleware
    {
        // Since we add a middleware, we need to ensure
        // the middleware handling has also been registered.
        return services
            .AddMiddlewareHandling()
            .AddSingleton<IMiddleware, TMiddleware>();
    }

    internal static IServiceCollection AddMiddlewareHandling(this IServiceCollection services)
    {
        services.TryAddSingleton<MiddlewareHandler>();
        return services;
    }
}
