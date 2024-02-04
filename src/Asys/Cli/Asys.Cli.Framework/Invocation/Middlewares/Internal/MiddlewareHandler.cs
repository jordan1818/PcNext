using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;

namespace Asys.Cli.Framework.Invocation.Middlewares.Internal;

/// <summary>
/// The service executing the registered <see cref="IMiddleware"/> as a chain-of-responsibility pattern.
/// </summary>
/// <remarks>
/// For more information about chain-of-responsibility, see <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern"/>.
/// </remarks>
public class MiddlewareHandler
{
    private readonly IEnumerable<IMiddleware> _middlewares;

    /// <summary>
    /// Initializes a new <see cref="MiddlewareHandler"/>.
    /// </summary>
    /// <param name="middlewares">The <see cref="IMiddleware"/> instances.</param>
    /// <param name="logger">The <see cref="ILogger"/> instance.</param>
    public MiddlewareHandler(IEnumerable<IMiddleware> middlewares, ILogger<MiddlewareHandler> logger)
        : this(middlewares, logger as ILogger) { }

    /// <inheritdoc/>
    /// <remarks>
    /// Ensure to use the templated <see cref="ILogger{T}"/> when binding the <paramref name="logger"/> value.
    /// </remarks>
    protected MiddlewareHandler(IEnumerable<IMiddleware> middlewares, ILogger logger)
    {
        _middlewares = middlewares;
        Logger = logger;
    }

    /// <summary>
    /// The <see cref="ILogger"/> instance.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Invoke the <see cref="IMiddleware"/>s as a chain-of-responsibility pattern.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> of the current CLI execution.</param>
    /// <param name="next">The next handler in CLI execution pipeline.</param>
    /// <returns>The executing <see cref="Task"/> done by the <see cref="IMiddleware"/>s.</returns>
    public virtual async Task InvokeAsync(InvocationContext context, Func<InvocationContext, Task> next)
    {
        // We order the middlewares by order from smallest to biggest
        // to respect the IMiddleware.Order intent.
        // We then build the chain-of-responsibility pattern and invoke the
        // "next" arg which is now our first middleware.
        // For more information about chain-of-responsibility, see:
        // https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern
        foreach (var middleware in _middlewares.OrderBy(m => m.Order).Reverse())
        {
            var originalNext = next;
            next = (c) => middleware.InvokeAsync(c, originalNext);
        }

        await next(context).ConfigureAwait(false);
    }
}
