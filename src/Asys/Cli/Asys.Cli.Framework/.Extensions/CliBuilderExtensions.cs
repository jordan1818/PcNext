using Asys.Cli.Framework.Configuration;
using Asys.Cli.Framework.Diagnostics.Logging;
using Asys.Cli.Framework.Internal;

namespace System.CommandLine.Builder;

/// <summary>
/// General extension methods for <see cref="CommandLineBuilder"/>.
/// </summary>
public static class CliBuilderExtensions
{
    /// <summary>
    /// Enables the default CLI framework behaviors for <paramref name="builder"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should be called only once else it might result in mis-behavior.
    /// </para>
    /// <para>
    /// If the <see cref="CommandLineBuilder.Command"/> does not have description, then it attempts to
    /// read the entry assmebly and get the relevant metadata instead.
    /// </para>
    /// It enables the following:
    /// <list type="bullet">
    /// <item>
    ///     <term><code>.UseDefaultRootCommand()</code></term>
    ///     <description>Sets the root command description to match the description of the project.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseDefaults()</code></term>
    ///     <description>to benefit from the default provided by System.CommandLine. For more information, see <see href="https://learn.microsoft.com/en-us/dotnet/standard/commandline/" />.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseDefaultAppConfiguration()</code></term>
    ///     <description>Enables the relevant configuration providers for your CLI such as <see cref="IEnvironmentVariables"/> and files.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseDefaultLogging()</code></term>
    ///     <description>Enables Serilog as default logger with file logging as well as console. For more information, see <see href="https://serilog.net/"/> for more information.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAsysCliDefaults(this CommandLineBuilder builder)
    {
        return builder
            .UseDefaultRootCommand()
            .UseDefaults()
            .UseDefaultAppConfiguration()
            .UseDefaultLogging();
    }

    /// <summary>
    /// Uses the default root command with no handler.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseDefaultRootCommand(this CommandLineBuilder builder)
    {
        return builder.ConfigureCommands(root =>
        {
            // Ensures a description is available to display in the help
            if (string.IsNullOrEmpty(root.Description))
            {
                root.SetDescriptionFromProjectInfo();
            }
        });
    }

    /// <summary>
    /// Allows to configure the <see cref="CommandLineBuilder.Command"/> and add more commands or groups of commands.
    /// </summary>
    /// <remarks>
    /// This method can be called multiple times, the results will be additive.
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="setup">The configure <see cref="CommandLineBuilder.Command"/> action.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ConfigureCommands(this CommandLineBuilder builder, Action<Command> setup)
    {
        // Ensures we hvae the builder as build-time-property
        // since it will be used when registering Commands with a CommandHandler.
        builder.Command.SetBuildTimeProperty(nameof(CommandLineBuilder), builder);

        // Configure the command using the provided setup action.
        builder.Command.ConfigureCommands(setup);

        return builder;
    }

    /// <summary>
    /// Execute a build step at most once based on the <paramref name="step"/> provided.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="step">The unique name of the step to execute only once.</param>
    /// <param name="setup">The action to execute only once.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder ExecuteOnlyOnce(this CommandLineBuilder builder, string step, Action setup)
    {
        // Gets or create the steptracker build-time-propery
        // and checks if the setp has already been added.
        // If not, the action is executed.
        var stepTracker = builder.Command.GetBuildTimeProperty("stepTracker", factory: () => new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        if (stepTracker.Add(step))
        {
            setup.Invoke();
        }

        return builder;
    }
}
