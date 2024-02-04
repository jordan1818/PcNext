using System.CommandLine;

namespace Asys.Cli.Framework.Internal;

/// <summary>
/// Defines an <see cref="Option"/> with no functional purpose, only
/// used to hold properties during the build time of the CLI.
/// </summary>
public class BuildTimeCustomPropertiesOption : Option<object?>
{
    /// <summary>
    /// The build time properties registered.
    /// </summary>
    public IDictionary<string, object?> BuildTimeProperties { get; } = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public BuildTimeCustomPropertiesOption()
        : base("BuildTimeCustomPropertiesOption")
    {
        // Since it has no functional prupose, then we
        // hide this option so it does not show in the help.
        IsHidden = true;
    }
}
