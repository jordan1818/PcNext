using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using Asys.Cli.Framework.Internal;
using Asys.Cli.Framework.Invocation.Internal;

namespace System.CommandLine;

/// <summary>
/// General extension methods for <see cref="Command"/>.
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Set the <see cref="Command.Handler"/> to a basic NoOp implementation.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> instance.</param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command SetNoOpHandler(this Command command)
    {
        // It would be nice to have the NoOp handler equal
        // to "--help" for nicer output and ensure the user is aware
        // this command has no implementation.
        command.SetHandler(() => { });
        return command;
    }

    /// <summary>
    /// Allows to configure a <see cref="Command"/> via a fluent like API.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> instance.</param>
    /// <param name="setup"></param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command ConfigureCommands(this Command command, Action<Command> setup)
    {
        setup.Invoke(command);
        return command;
    }

    /// <summary>
    /// Add a <see cref="Command"/> as a child to a command group or <see cref="RootCommand"/>.
    /// </summary>
    /// <typeparam name="TCommand">The <see cref="Command"/> type.</typeparam>
    /// <param name="command">The <see cref="Command"/> instance.</param>
    /// <param name="childSetup">The setup used for chaining based on the specified <typeparamref name="TCommand"/>.</param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command AddCommand<TCommand>(this Command command, Action<Command>? childSetup = null)
        where TCommand : Command, new()
    {
        // Creates the new command and add
        // it as a child to the parent command
        var newCommand = new TCommand();
        command.Add(newCommand);

        // If the command has a handler defined, then
        // we register the handler as a service in the DI container
        // provided by the built-in Host
        if (newCommand is ICommandHasHandler commandWithHandler)
        {
            // Set a default handler, else the nesting of commands
            // does not work as the parser is not aware this command
            // has an implementation until it reaches the DI resolution.
            // see `hostBuilder.UseCommandHandler` for more details.
            if (newCommand.Handler is null)
            {
                newCommand.SetNoOpHandler();
            }

            var commandLineBuilder = command.GetRequiredBuildTimeProperty<CommandLineBuilder>(nameof(CommandLineBuilder));
            commandLineBuilder.ConfigureHosting((context, hostBuilder) => hostBuilder.UseCommandHandler(typeof(TCommand), commandWithHandler.CommandHandlerType));
        }

        // If a setup has been provided, then we allow for
        // fluent API like chaining by calling the setup with
        // the newly created instance as arg.
        if (childSetup is not null)
        {
            newCommand.ConfigureCommands(childSetup);
        }

        return command;
    }

    /// <inheritdoc cref="AddCommand(Command, string, string?, Action{Command}?)"/>
    public static Command AddCommand(this Command command, string name, Action<Command>? childSetup = null)
    {
        return command.AddCommand(name, description: null, childSetup: childSetup);
    }

    /// <summary>
    /// Add a command group as a child to a command group or <see cref="RootCommand"/>.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> instance.</param>
    /// <param name="name">The command group name.</param>
    /// <param name="description">The command group description.</param>
    /// <param name="childSetup">The setup used for chaining based on the specified <typeparamref name="TCommand"/>.</param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command AddCommand(this Command command, string name, string? description, Action<Command>? childSetup = null)
    {
        // Create a new command with the specified name and description.
        // Since there is no handler setup, this is considered a command group.
        // It is then added to the parent command.
        var newCommand = new Command(name, description);
        command.Add(newCommand);

        // If a setup has been provided, then we allow for
        // fluent API like chaining by calling the setup with
        // the newly created instance as arg.
        if (childSetup is not null)
        {
            newCommand.ConfigureCommands(childSetup);
        }

        return command;
    }
}
