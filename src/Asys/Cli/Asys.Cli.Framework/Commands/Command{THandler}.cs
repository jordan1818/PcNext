using System.CommandLine.Invocation;
using Asys.Cli.Framework.Invocation.Internal;

namespace System.CommandLine;

/// <summary>
/// A <see cref="Command"/> associated with a <see cref="ICommandHandler"/>.
/// </summary>
/// <typeparam name="THandler">The <see cref="ICommandHandler"/> type.</typeparam>
public class Command<THandler> : Command, ICommandHasHandler
    where THandler : class, ICommandHandler
{
    /// <inheritdoc/>
    public Command(string name, string? description = null)
        : base(name, description)
    {
    }

    /// <inheritdoc/>
    public Type CommandHandlerType => typeof(THandler);
}
