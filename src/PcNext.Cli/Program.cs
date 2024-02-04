using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PcNext.Tests")]

namespace PcNext.Cli;

internal static class Program
{
    private static Task<int> Main(string[] args)
    {
        var cliBuilder = new CommandLineBuilder()
                .AddApplication();

        var cli = cliBuilder.Build();
        return cli.InvokeAsync(args);
    }

    internal static CommandLineBuilder AddApplication(this CommandLineBuilder builder) 
        => builder
            .UseAsysCliDefaults()
            .UseAsysDefaults()
            .UseChocolateySharp()
            .UsePcNextDefaults()
            .UsePcNextConfigureIntegrations();
}
