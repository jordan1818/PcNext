using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Configuration;

namespace Asys.Cli.Framework.Configuration.Commands.Options;

/// <summary>
/// Defines a <see cref="Option"/> to specify a custom key=value pairs bound to <see cref="IConfiguration"/>.
/// </summary>
/// <remarks>
/// When added as an <see cref="Option"/>, it will be available as "--config".
/// </remarks>
public class ConfigurationKvpOverrideCommandOption : Option<IEnumerable<KeyValuePair<string, string?>>>
{
    /// <summary>
    /// Initializes a new <see cref="ConfigurationKvpOverrideCommandOption"/>
    /// </summary>
    public ConfigurationKvpOverrideCommandOption()
        : base(aliases: new[] { "--config", "-c" }, description: "Allows to override the default config by specifying 'key=value'", parseArgument: ParseArgument)
    {
#pragma warning disable MA0056 // Do not call overridable members in constructor
        Arity = ArgumentArity.OneOrMore;
#pragma warning restore MA0056 // Do not call overridable members in constructor
    }

    // For more information on how to parse argument and options, see:
    // https://learn.microsoft.com/en-us/dotnet/standard/commandline/model-binding#custom-validation-and-binding
    private static List<KeyValuePair<string, string?>> ParseArgument(ArgumentResult result)
    {
        // We output as a list to ensure the collection is enumerated only once.
        var output = new List<KeyValuePair<string, string?>>();

        foreach (var token in result.Tokens)
        {
            // We attempt to split the token, eg. 'key=value'
            // and get the first segment which should be the key.
            var kvpArray = token.Value?.Split('=');

            string? key = null;
            if (kvpArray?.Length > 0)
            {
                key = kvpArray[0];
            }

            // If we have found a key, then the config token
            // is valid and we can continue by parsing the value.
            if (!string.IsNullOrWhiteSpace(key))
            {
                // Since the value might contain a '=', or the
                // kvp might contain only the key eg. 'key=' or 'key'
                // then we adapt the substring accordingly.
                var substringLength = key.Length;
                if (kvpArray!.Length > 1)
                {
                    substringLength++;
                }

                var value = token.Value![substringLength..];

                // We support an empty or null value, so we can add the kvp safely to the output.
                output.Add(new KeyValuePair<string, string?>(key, value));
            }
        }

        // We retyurn the output which can be red
        // by using context.ParseResult.GetValueForOption(...)
        return output;
    }
}
