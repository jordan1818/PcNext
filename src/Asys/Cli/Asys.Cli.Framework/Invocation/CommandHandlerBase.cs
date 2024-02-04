namespace System.CommandLine.Invocation;

/// <summary>
/// Define a base <see cref="ICommandHandler"/> used to standardize the implementation.
/// </summary>
public abstract class CommandHandlerBase : ICommandHandler
{
    /// <summary>
    /// Initializes a new <see cref="CommandHandlerBase"/>.
    /// </summary>
    protected CommandHandlerBase()
    {

    }

    /// <inheritdoc cref="InvokeAsync(InvocationContext)"/>
    public int Invoke(InvocationContext context)
    {
        return InvokeAsync(context).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public abstract Task<int> InvokeAsync(InvocationContext context);
}
