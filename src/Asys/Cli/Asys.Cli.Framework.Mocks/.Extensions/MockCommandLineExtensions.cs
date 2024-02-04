using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Asys.Cli.Framework.Mocks;
using Asys.Cli.Framework.Mocks.Internal;
using Asys.Cli.Framework.Mocks.Invocation;
using Asys.System.Environment;

namespace System.CommandLine.Builder;

public static class MockCommandLineExtensions
{
    public static CommandLineBuilder UseMockEnvironmentVariables(this CommandLineBuilder builder, Action<IEnvironmentVariables>? setup = null)
    {
        return builder.AddMiddleware(
            order: (MiddlewareOrder)int.MinValue,
            middleware: async (context, next) =>
            {
                var inMemoryEnvironmentVariables = context.BindingContext.GetService<InMemoryEnvironmentVariables>();
                if (inMemoryEnvironmentVariables == null)
                {
                    // Get any other environment variable services
                    // to retrieve for in memory environment variables
                    // to use.
                    var environmentVariables = context.BindingContext.GetService<IEnvironmentVariables>();

                    inMemoryEnvironmentVariables = new InMemoryEnvironmentVariables(environmentVariables);
                    context.BindingContext.AddService<IEnvironmentVariables>(_ => inMemoryEnvironmentVariables);
                }

                setup?.Invoke(inMemoryEnvironmentVariables);

                await next(context).ConfigureAwait(false);
            });
    }

    public static async Task<MockInvocationResult> InvokeMockAsync(this Parser parser, string command, MockConsoleOptions? options = null)
    {
        var originalConsoleErr = Console.Error;
        var originalConsoleOut = Console.Out;

        var console = new MockConsole(options);

#pragma warning disable CA2000 // Dispose objects before losing scope
        Console.SetOut(new StandardStreamTextWriter(console.Out));
        Console.SetError(new StandardStreamTextWriter(console.Error));
#pragma warning restore CA2000 // Dispose objects before losing scope

        var exitCode = await parser.InvokeAsync(command, console).ConfigureAwait(false);

        Console.SetOut(originalConsoleOut);
        Console.SetError(originalConsoleErr);

        return new MockInvocationResult(exitCode, console);
    }
}
