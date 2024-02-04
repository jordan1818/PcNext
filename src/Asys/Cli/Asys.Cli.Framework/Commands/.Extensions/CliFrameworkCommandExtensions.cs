using System.Reflection;

namespace System.CommandLine;

/// <summary>
/// General extension methods for <see cref="Command"/>.
/// </summary>
public static class CliFrameworkCommandExtensions
{
    /// <summary>
    /// Sets the command description to match the project information.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> instance.</param>
    /// <returns>The <paramref name="command"/> instance for chaining.</returns>
    public static Command SetDescriptionFromProjectInfo(this Command command)
    {
        command.Description = Assembly
            .GetEntryAssembly()?
            .GetCustomAttribute<AssemblyDescriptionAttribute>()?
            .Description ?? command.Description ?? string.Empty;

        return command;
    }
}
