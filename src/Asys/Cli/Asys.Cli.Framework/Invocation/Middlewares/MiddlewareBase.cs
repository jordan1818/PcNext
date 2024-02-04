using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;

namespace Asys.Cli.Framework.Invocation.Middlewares;

/// <summary>
/// Defines a base <see cref="IMiddleware"/> used to standardize the implementation.
/// </summary>
/// <remarks>
/// While you can override <see cref="IMiddleware.InvokeAsync(InvocationContext, Func{InvocationContext, Task})"/>, this
/// base class also offers two extension points:
/// <list type="bullet">
/// <item>
/// <term><see cref="OnBeforeNextAsync"/></term>
/// <description>Triggered before calling the next link in the chain-of-responsibility. By default it only logs a debug message.</description>
/// </item>
/// <item>
/// <term><see cref="OnAfterNextAsync"/></term>
/// <description>Triggered after calling the next link in the chain-of-responsibility. By default it only logs a debug message.</description>
/// </item>
/// </list>
/// </remarks>
public abstract class MiddlewareBase : IMiddleware
{
    /// <inheritdoc/>
    public virtual int Order => 0;

    /// <summary>
    /// The <see cref="ILogger"/> instance.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Initializes a new <see cref="MiddlewareBase"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> instance.</param>
    /// <remarks>
    /// Ensure to use the templated <see cref="ILogger{T}"/> when binding the <paramref name="logger"/> value.
    /// </remarks>
    protected MiddlewareBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Triggered before calling the next link in the chain-of-responsibility.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> of the current CLI execution.</param>
    /// <returns>The executing <see cref="Task"/> done by the <see cref="IMiddleware"/>.</returns>
    protected virtual Task OnBeforeNextAsync(InvocationContext context)
    {
        Logger.LogDebug(nameof(OnBeforeNextAsync));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Triggered after calling the next link in the chain-of-responsibility.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> of the current CLI execution.</param>
    /// <returns>The executing <see cref="Task"/> done by the <see cref="IMiddleware"/>.</returns>
    protected virtual Task OnAfterNextAsync(InvocationContext context)
    {
        Logger.LogDebug(nameof(OnAfterNextAsync));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual async Task InvokeAsync(InvocationContext context, Func<InvocationContext, Task> next)
    {
        await OnBeforeNextAsync(context).ConfigureAwait(false);
        await next(context).ConfigureAwait(false);
        await OnAfterNextAsync(context).ConfigureAwait(false);
    }
}
