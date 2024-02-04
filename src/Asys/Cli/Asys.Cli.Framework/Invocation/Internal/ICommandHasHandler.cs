using System.CommandLine;
using System.CommandLine.Invocation;

namespace Asys.Cli.Framework.Invocation.Internal;

/// <summary>
/// Defines a <see cref="Command"/> with a <see cref="ICommandHandler"/>.
/// </summary>
public interface ICommandHasHandler
{
    /// <summary>
    /// The <see cref="ICommandHandler"/> type associated with the <see cref="Command"/>.
    /// </summary>
    Type CommandHandlerType { get; }
}
