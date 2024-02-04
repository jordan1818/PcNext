using Asys.System.Diagnostics;
using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;
using ChocolateySharp.Internal.Tasks.Contexts;
using ChocolateySharp.Options;
using Microsoft.Extensions.Logging;

namespace ChocolateySharp;

/// <summary>
/// An implementation of <see cref="IChocolateyClient"/>.
/// </summary>
public sealed class ChocolateyClient : IChocolateyClient
{
    private bool _alreadyValidatedInstalledChocolatey = false;

    private readonly ILogger _logger;
    private readonly IProcessTaskFactory _processTaskFactory;

    /// <summary>
    /// Initializes an instance of <see cref="ChocolateyClient"/>.
    /// </summary>
    /// <param name="logger">The logger for all <see cref="ChocolateyClient"/> operations.</param>
    public ChocolateyClient(ILogger logger)
        : this(logger, processTaskFactory: null)
    {
    }

    /// <summary>
    /// Initializes an instance of <see cref="ChocolateyClient"/>.
    /// </summary>
    /// <param name="logger">The logger for all <see cref="ChocolateyClient"/> operations.</param>
    /// <param name="processFactory">The instance of <see cref="IProcessFactory"/> for process operations.</param>
    internal ChocolateyClient(ILogger logger, IProcessTaskFactory? processTaskFactory)
    {
        _logger = logger;
        _processTaskFactory = processTaskFactory ?? new ProcessTaskFactory();
    }

    private Task<ProcessTaskResults> ExecutePowerShellProcessTask(PowerShellProcessTaskContext context)
    {
        return _processTaskFactory.Create().ExecuteAsync(context);
    }

    private Task<bool> ValidateChocoCommandAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        { 
            var results = await ExecutePowerShellProcessTask(new PowerShellProcessTaskContext(_logger)
            {
                Arguments = "try { if(Get-Command choco) { exit 0 } catch { exit 1 }",
                CancellationToken = cancellationToken,
            });

            return results.IsSuccessful;
         });
    }

    private Task InstallChocoCommandAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            var results = await ExecutePowerShellProcessTask(new PowerShellProcessTaskContext(_logger)
            {
                Arguments = "[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))",
                CancellationToken = cancellationToken,
            });

            if (!results.IsSuccessful)
            {
                throw new InvalidOperationException(results.FailureMessage, results.Exception);
            }
        });
    }

    private async Task<ProcessTaskResults> ExecuteChocoCommandAsync(ChocoProcessTaskContext chocoProcessTaskContext)
    {
        if (!_alreadyValidatedInstalledChocolatey && !await ValidateChocoCommandAsync(chocoProcessTaskContext.CancellationToken))
        {
            await InstallChocoCommandAsync(chocoProcessTaskContext.CancellationToken);

            _alreadyValidatedInstalledChocolatey = true;
        }

        return await _processTaskFactory.Create().ExecuteAsync(chocoProcessTaskContext);
    }

    /// <inheritdoc/>
    public Task<bool> InstallPackageAsync(string name, ChocolateyPackageInstallOptions? options = null, CancellationToken cancellationToken = default)
    {
        return Task.Run(async () =>
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("'{NameAgument}' is not set.", nameof(name));
                return false;
            }

            var results = await ExecuteChocoCommandAsync(new ChocoInstallPackageProcessTaskContext(_logger, name, options)
            {
                CancellationToken = cancellationToken,
            });
            return results.IsSuccessful;
        });
    }
}
