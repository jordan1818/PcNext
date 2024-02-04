using System.CommandLine.Parsing;

namespace System.CommandLine;

/// <summary>
/// A <see cref="bool"/> <see cref="Option"/> configured as a switch.
/// </summary>
#pragma warning disable MA0056 // Do not call overridable members in constructor
public class SwitchOption : Option<bool>
{
    /// <inheritdoc/>
    public SwitchOption(string name, string? description = null)
        : this(name, () => false, description)
    {
    }

    /// <inheritdoc/>
    public SwitchOption(string[] aliases, string? description = null)
        : this(aliases, () => false, description)
    {
    }

    /// <inheritdoc/>
    public SwitchOption(string name, Func<bool> getDefaultValue, string? description = null)
        : base(name, getDefaultValue, description)
    {
        Arity = ArgumentArity.Zero;
    }

    /// <inheritdoc/>
    public SwitchOption(string[] aliases, Func<bool> getDefaultValue, string? description = null)
        : base(aliases, getDefaultValue, description)
    {
        Arity = ArgumentArity.Zero;
    }

    /// <inheritdoc/>
    public SwitchOption(string name, ParseArgument<bool> parseArgument, bool isDefault = false, string? description = null)
        : base(name, parseArgument, isDefault, description)
    {
        Arity = ArgumentArity.Zero;
    }

    /// <inheritdoc/>
    public SwitchOption(string[] aliases, ParseArgument<bool> parseArgument, bool isDefault = false, string? description = null)
        : base(aliases, parseArgument, isDefault, description)
    {
        Arity = ArgumentArity.Zero;
    }
}
#pragma warning restore MA0056 // Do not call overridable members in constructor
