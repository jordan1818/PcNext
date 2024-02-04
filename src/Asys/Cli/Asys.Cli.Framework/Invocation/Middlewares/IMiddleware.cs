using System.CommandLine.Invocation;

namespace Asys.Cli.Framework.Invocation.Middlewares;

/// <summary>
/// Defines a middleware to interecpt the CLI execution pipeline as a chain-of-responsibility link.
/// </summary>
/// <remarks>
/// For more information about chain-of-responsibility, see <see href="https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern"/>.
/// </remarks>
public interface IMiddleware
{
    /// <summary>
    /// Defines the order at which the <see cref="IMiddleware"/> will be executed.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Invoke the <see cref="IMiddleware"/> logic as a chain-of-responsibility link.
    /// </summary>
    /// <param name="context">The <see cref="InvocationContext"/> of the current CLI execution.</param>
    /// <param name="next">The next handler in the chain-of-responsibility.</param>
    /// <returns>The executing <see cref="Task"/> done by the <see cref="IMiddleware"/>.</returns>
    Task InvokeAsync(InvocationContext context, Func<InvocationContext, Task> next);
}
