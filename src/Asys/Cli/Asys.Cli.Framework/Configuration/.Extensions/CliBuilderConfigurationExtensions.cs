using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Asys.Cli.Framework.Configuration.Commands.Options;
using Asys.Cli.Framework.System.Internal;
using Asys.System.Environment;

namespace Asys.Cli.Framework.Configuration;

/// <summary>
/// Configuration extension methods for <see cref="CommandLineBuilder"/>.
/// </summary>
public static class CliBuilderConfigurationExtensions
{
    /// <summary>
    /// Enables the default CLI framework configuration behaviors for <paramref name="builder"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method should be called only once else it might result in mis-behavior.
    /// </para>
    /// It enables the following:
    /// <list type="bullet">
    /// <item>
    ///     <term><code>.UseAppDataAppConfiguration()</code></term>
    ///     <description>Binds the config files from the different AppData folders.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseContentRootPathAppConfiguration()</code></term>
    ///     <description>Binds the config file from the content root folder.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseWorkingDirectoryAppConfiguration()</code></term>
    ///     <description>Binds the config file from the working directory.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseEnvironmentVariablesAppConfiguration()</code></term>
    ///     <description>Binds the environment variables.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseFileOverrideAppConfiguration()</code></term>
    ///     <description>Binds the user specified files.</description>
    /// </item>
    /// <item>
    ///     <term><code>.UseKvpOverrideAppConfiguration()</code></term>
    ///     <description>Binds the user specified key=value pairs.</description>
    /// </item>
    /// </list>
    /// <para>
    /// For more information about <see cref="IConfiguration"/>, see <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0"/>.
    /// </para>
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseDefaultAppConfiguration(this CommandLineBuilder builder)
    {
        return builder
            .UseFileOverrideAppConfiguration()
            .UseKvpOverrideAppConfiguration()
            .UseAppDataAppConfiguration()
            .UseContentRootPathAppConfiguration()
            .UseWorkingDirectoryAppConfiguration()
            .UseEnvironmentVariablesAppConfiguration()
            .UseFileOverrideAppConfiguration()
            .UseKvpOverrideAppConfiguration();
    }

    /// <summary>
    /// Adds the JSON configuration file present in the app data directories as <see cref="IConfigurationSource"/>.
    /// </summary>
    /// <remarks>
    /// It adds the following JSON configuration file if present in the following directories:
    /// <list type="bullet">
    /// <item><code>{Environment.SpecialFolder.CommonApplicationData}/{app-name}/config/{app-name}.config</code></item>
    /// <item><code>{Environment.SpecialFolder.ApplicationData}/{app-name}/config/{app-name}.config</code></item>
    /// <item><code>{Environment.SpecialFolder.LocalApplicationData}/{app-name}/config/{app-name}.config</code></item>
    /// </list>
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseAppDataAppConfiguration(this CommandLineBuilder builder)
    {
        return builder.ConfigureAppConfiguration((context, host, configBuilder) =>
        {
            var allUsersConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), $"{host.HostingEnvironment.ApplicationName}/config/{host.HostingEnvironment.ApplicationName}.config");
            configBuilder.AddJsonFile(allUsersConfig, optional: true, reloadOnChange: false);

            var currentRoamingUserConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{host.HostingEnvironment.ApplicationName}/config/{host.HostingEnvironment.ApplicationName}.config");
            configBuilder.AddJsonFile(currentRoamingUserConfig, optional: true, reloadOnChange: false);

            var currentLocalUserConfig = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{host.HostingEnvironment.ApplicationName}/config/{host.HostingEnvironment.ApplicationName}.config");
            configBuilder.AddJsonFile(currentLocalUserConfig, optional: true, reloadOnChange: false);
        });
    }

    /// <summary>
    /// Adds the JSON configuration file present in the application directory as a <see cref="IConfigurationSource"/>.
    /// </summary>
    /// <remarks>
    /// It adds the following JSON configuration file if present in the following directory:
    /// <list type="bullet">
    /// <item><code>{ContentRootPath}/{app-name}.config</code></item>
    /// </list>
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseContentRootPathAppConfiguration(this CommandLineBuilder builder)
    {
        return builder.ConfigureAppConfiguration((context, host, configBuilder) => configBuilder.AddJsonFile($"{host.HostingEnvironment.ContentRootPath}/{host.HostingEnvironment.ApplicationName}.config", optional: true, reloadOnChange: false));
    }

    /// <summary>
    /// Adds the JSON configuration file present in the working directory as a <see cref="IConfigurationSource"/>.
    /// </summary>
    /// <remarks>
    /// It adds the following JSON configuration file if present in the following directory:
    /// <list type="bullet">
    /// <item><code>./{app-name}.config</code></item>
    /// </list>
    /// </remarks>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseWorkingDirectoryAppConfiguration(this CommandLineBuilder builder)
    {
        return builder.ConfigureAppConfiguration((context, host, configBuilder) => configBuilder.AddJsonFile($"./{host.HostingEnvironment.ApplicationName}.config", optional: true, reloadOnChange: false));
    }

    /// <inheritdoc cref="UseEnvironmentVariablesAppConfiguration(CommandLineBuilder, string)"/>
    public static CommandLineBuilder UseEnvironmentVariablesAppConfiguration(this CommandLineBuilder builder)
    {
        return builder.ConfigureAppConfiguration((context, host, configBuilder) => {
            var environmentVariables = context.BindingContext.GetService<IEnvironmentVariables>() ?? new EnvironmentVariables();
            configBuilder.Add(new CustomEnvironmentVariablesConfigurationSource(environmentVariables));
        });
    }

    /// <summary>
    /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <param name="prefix">The prefix that environment variable names must start with. The prefix will be removed from the environment variable names.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseEnvironmentVariablesAppConfiguration(this CommandLineBuilder builder, string prefix)
    {
        return builder.ConfigureAppConfiguration((context, host, configBuilder) => {
            var environmentVariables = context.BindingContext.GetService<IEnvironmentVariables>() ?? new EnvironmentVariables();
            configBuilder.Add(new CustomEnvironmentVariablesConfigurationSource(environmentVariables) { Prefix = prefix });
        });
    }

    /// <summary>
    /// Enables the <see cref="ConfigurationFileOverrideCommandOption"/> to be used a <see cref="IConfiguration"/> source.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseFileOverrideAppConfiguration(this CommandLineBuilder builder)
    {
        // We add the necessary option
        builder.AddFileConfigurationOverrideOptions();

        // We add a config handler that will bind
        // the specified files to the config.
        builder.ConfigureAppConfiguration((context, host, configBuilder) =>
        {
            var configFileOverrideCommandOption = context.BindingContext.GetRequiredService<ConfigurationFileOverrideCommandOption>();
            var fileInfos = context.ParseResult.GetValueForOption(configFileOverrideCommandOption);
            if (fileInfos != null)
            {
                foreach (var fileInfo in fileInfos)
                {
                    configBuilder.AddJsonFile(fileInfo.FullName, optional: true, reloadOnChange: false);
                }
            }
        });

        return builder;
    }

    private static CommandLineBuilder AddFileConfigurationOverrideOptions(this CommandLineBuilder builder)
    {
        // Since we add global options, we want to ensure this is called only once
        // and avoid mis-behavior in case it is called multiple times.
        return builder.ExecuteOnlyOnce(nameof(AddFileConfigurationOverrideOptions), () =>
        {
            var configFileOverrideCommandOption = new ConfigurationFileOverrideCommandOption();
            builder.Command.AddGlobalOption(configFileOverrideCommandOption);

            builder.ConfigureHosting((context, hostBuilder) => context.BindingContext.AddService(_ => configFileOverrideCommandOption));
            builder.ConfigureServices((context, host, services) => services.AddSingleton(configFileOverrideCommandOption));
        });
    }

    /// <summary>
    /// Enables the <see cref="ConfigurationKvpOverrideCommandOption"/> to be used a <see cref="IConfiguration"/> source.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> instance.</param>
    /// <returns>The <paramref name="builder"/> instance for chaining.</returns>
    public static CommandLineBuilder UseKvpOverrideAppConfiguration(this CommandLineBuilder builder)
    {
        // We add the necessary option
        builder.AddKvpConfigurationOverrideOptions();

        // We add a config handler that will parse the kvp values
        // and bind them the config via an in-memory collection.
        builder.ConfigureAppConfiguration((context, host, configBuilder) =>
        {
            var configKvpOverrideCommandOption = context.BindingContext.GetRequiredService<ConfigurationKvpOverrideCommandOption>();
            var kvps = context.ParseResult.GetValueForOption(configKvpOverrideCommandOption);
            if (kvps?.Any() == true)
            {
                configBuilder.AddInMemoryCollection(kvps);
            }
        });

        return builder;
    }

    private static CommandLineBuilder AddKvpConfigurationOverrideOptions(this CommandLineBuilder builder)
    {
        // Since we add global options, we want to ensure this is called only once
        // and avoid mis-behavior in case it is called multiple times.
        return builder.ExecuteOnlyOnce(nameof(AddKvpConfigurationOverrideOptions), () =>
        {
            var configKvpOverrideCommandOption = new ConfigurationKvpOverrideCommandOption();
            builder.Command.AddGlobalOption(configKvpOverrideCommandOption);

            builder.ConfigureHosting((context, hostBuilder) => context.BindingContext.AddService(_ => configKvpOverrideCommandOption));
            builder.ConfigureServices((context, host, services) => services.AddSingleton(configKvpOverrideCommandOption));
        });
    }
}
