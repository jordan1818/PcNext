using System.CommandLine;

namespace Asys.Cli.Framework.Internal;

/// <summary>
/// Generic extensions relatedd to <see cref="RootCommand"/>.
/// </summary>
public static class RootCommandExtensions
{
    /// <summary>
    /// Retrieve all the build time properties registered.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> holding the build time properties.</param>
    /// <returns>The found build time properties.</returns>
    public static IDictionary<string, object?> GetBuildTimeProperties(this Command command)
    {
        // Get the Option holding the build time properties, or add it if missing.
        if (command.Options.FirstOrDefault(o => o is BuildTimeCustomPropertiesOption) is not BuildTimeCustomPropertiesOption buildTimePropertiesOption)
        {
            buildTimePropertiesOption = new BuildTimeCustomPropertiesOption();
            command.AddOption(buildTimePropertiesOption);
        }

        return buildTimePropertiesOption.BuildTimeProperties;
    }

    /// <summary>
    /// Retrieve a build time property.
    /// </summary>
    /// <typeparam name="TProperty">The build time property type.</typeparam>
    /// <param name="command">The <see cref="Command"/> holding the build time properties.</param>
    /// <param name="key">The build time property key.</param>
    /// <param name="factory">The build time factory in case the build time property does not exist.</param>
    /// <returns>The build time property.</returns>
    public static TProperty GetBuildTimeProperty<TProperty>(this Command command, string key, Func<TProperty> factory)
    {
        // We only want to deal with the root command since
        // the build time properties should be associated only
        // to the main command.
        var rootCommand = command.GetRequiredRootCommand();

        // We retrieve the build time property and if it is not
        // present, we use the factory to create it and add it.
        var buildTimeProperties = rootCommand.GetBuildTimeProperties();
        if (!buildTimeProperties.TryGetValue(key, out var oProperty))
        {
            oProperty = factory.Invoke();
            buildTimeProperties.Add(key, oProperty);
        }

        return (TProperty)oProperty!;
    }

    /// <summary>
    /// Sets a build time property.
    /// </summary>
    /// <typeparam name="TProperty">The build time property type.</typeparam>
    /// <param name="command">The <see cref="Command"/> holding the build time properties.</param>
    /// <param name="key">The build time property key.</param>
    /// <param name="property">The build time property value.</param>
    public static void SetBuildTimeProperty<TProperty>(this Command command, string key, TProperty property)
    {
        // We only want to deal with the root command since
        // the build time properties should be associated only
        // to the main command.
        var rootCommand = command.GetRequiredRootCommand();

        // We retrieve the build time properties and
        // set the value as specified by the caller.
        var buildTimeProperties = rootCommand.GetBuildTimeProperties();
        buildTimeProperties[key] = property;
    }

    /// <summary>
    /// Retrieve a build time property.
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="command">The <see cref="Command"/> holding the build time properties.</param>
    /// <param name="key">The build time property key.</param>
    /// <returns>The build time property.</returns>
    /// <exception cref="KeyNotFoundException">The build time property was not found.</exception>
    public static TProperty GetRequiredBuildTimeProperty<TProperty>(this Command command, string key)
    {
        var rootCommand = command.GetRequiredRootCommand();

        return rootCommand.GetBuildTimeProperty<TProperty>(key, () => throw new KeyNotFoundException(key));
    }

    /// <summary>
    /// Get the <see cref="RootCommand"/> from the specified <paramref name="command"/>.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> to find the <see cref="RootCommand"/> from.</param>
    /// <returns>The found <see cref="RootCommand"/>.</returns>
    /// <exception cref="KeyNotFoundException">The <see cref="RootCommand"/> was not found.</exception>
    public static RootCommand GetRequiredRootCommand(this Command command)
    {
        return command.GetRootCommand() ?? throw new KeyNotFoundException(nameof(RootCommand));
    }

    /// <summary>
    /// Get the <see cref="RootCommand"/> from the specified <paramref name="command"/>.
    /// </summary>
    /// <param name="command">The <see cref="Command"/> to find the <see cref="RootCommand"/> from.</param>
    /// <returns>The found <see cref="RootCommand"/> or null if not found.</returns>
    public static RootCommand? GetRootCommand(this Command command)
    {
        if (command is RootCommand rc)
        {
            return rc;
        }

        foreach (var parent in command.Parents)
        {
            if (parent is Command c)
            {
                var frc = GetRootCommand(c);
                if (frc != null)
                    return frc;
            }
        }

        return null;
    }
}
