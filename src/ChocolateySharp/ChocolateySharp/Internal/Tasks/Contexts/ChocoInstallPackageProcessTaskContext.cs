using ChocolateySharp.Options;
using Microsoft.Extensions.Logging;

namespace ChocolateySharp.Internal.Tasks.Contexts;

internal sealed class ChocoInstallPackageProcessTaskContext : ChocoProcessTaskContext
{
    private readonly string _name;
    private readonly ChocolateyPackageInstallOptions? _options;

    public ChocoInstallPackageProcessTaskContext(ILogger logger, string name, ChocolateyPackageInstallOptions? options)
        : base(logger)
    {
        _name = name;
        _options = options;
    }

    public override string? FullArugments
    {
        get
        {
            var argumentList = new List<string>
            {
                "install",

                // Enforce confirmation of installing Chocolatey package.
                // This will stop and wait for an input of the client.
                // This is not handled and should be enforce.
                "--confirm",

                _name,
            };


            if (_options?.Version is not null)
            {
                argumentList.Add("--version " + _options.Version.ToString());
            }

            if (!string.IsNullOrWhiteSpace(_options?.PackageParameters))
            {
                argumentList.Add($"--package-parameters \"{_options.PackageParameters}\"");
            }

            if (!string.IsNullOrWhiteSpace(Arguments))
            {
                argumentList.Add(Arguments);
            }

            return string.Join(" ", argumentList);
        }
    }
}
