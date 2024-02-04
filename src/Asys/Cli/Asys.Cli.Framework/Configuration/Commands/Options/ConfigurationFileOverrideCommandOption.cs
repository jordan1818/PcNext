using System.CommandLine;
using Microsoft.Extensions.Configuration;

namespace Asys.Cli.Framework.Configuration.Commands.Options;

/// <summary>
/// Defines a <see cref="Option"/> to specify a custom configuration file bound to <see cref="IConfiguration"/>.
/// </summary>
/// <remarks>
/// When added as an <see cref="Option"/>, it will be available as "--config-file".
/// </remarks>
public class ConfigurationFileOverrideCommandOption : Option<IEnumerable<FileInfo>>
{
    /// <summary>
    /// Initializes a new <see cref="ConfigurationFileOverrideCommandOption"/>
    /// </summary>
    public ConfigurationFileOverrideCommandOption()
        : base(new[] { "--config-file" }, "Allows to override the default config by specifying a json config file path")
    {
#pragma warning disable MA0056 // Do not call overridable members in constructor
        Arity = ArgumentArity.OneOrMore;
#pragma warning restore MA0056 // Do not call overridable members in constructor

        this.LegalFilePathsOnly();

        // Since the user takes the decision to specify a file, we assume
        // the file must exists. This allows the user to be aware their input was
        // incorrect. If they would like to not have this validation, they could
        // use the working directory config file or the app data ones.
        AddValidator(result =>
        {
            var fileInfos = result.GetValueForOption(this);
            if (fileInfos != null)
            {
                foreach (var fileInfo in fileInfos)
                {
                    if (!fileInfo.Exists)
                    {
                        // If the file does not exist, we do not check the others and simply exit right away.
                        // NTH: it might be interesting to have a string builder and list all incriminated files.
                        result.ErrorMessage = $"Config file specified '{fileInfo.FullName}' does not exist. Ensure to specify an existing file when using '--config-file'.";
                        return;
                    }
                }
            }
        });
    }
}
