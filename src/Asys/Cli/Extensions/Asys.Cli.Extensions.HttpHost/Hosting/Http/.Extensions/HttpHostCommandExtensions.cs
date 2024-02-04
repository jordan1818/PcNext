using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Asys.Cli.Framework.Internal;

namespace System.CommandLine;

/// <summary>
/// <see cref="IWebHost"/> extension methods for <see cref="Command"/>.
/// </summary>
public static class HttpHostCommandExtensions
{
    /// <summary>
    /// Configures the <see cref="IWebHost"/> started alongside the command execution.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> on which the HTTP host will be enabled.</param>
    /// <param name="setup">The setup to configure the <see cref="IWebHost"/>.</param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command ConfigureWebHost(this Command command, Action<InvocationContext, IHostBuilder, IWebHostBuilder> setup)
    {
        var builder = command.GetRequiredBuildTimeProperty<CommandLineBuilder>(nameof(CommandLineBuilder));
        builder.ConfigureWebHost(
            predicate: context => context.ParseResult.CommandResult.Command == command,
            setup: setup);

        return command;
    }

    /// <summary>
    /// Enables the <see cref="IWebHost"/> when this command execute.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> on which the HTTP host will be enabled.</param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command EnableWebHost(this Command command)
    {
        var builder = command.GetRequiredBuildTimeProperty<CommandLineBuilder>(nameof(CommandLineBuilder));
        builder.EnableWebHost(context => context.ParseResult.CommandResult.Command.IsOrItsParentCommand(c => c == command));

        return command;
    }

    private static bool IsOrItsParentCommand(this Command command, Func<Command, bool> predicate)
    {
        if (predicate(command))
        {
            return true;
        }

        return command.Parents.FirstOrDefault(p => p is Command) is Command parentCommand && IsOrItsParentCommand(parentCommand, predicate);
    }
}
