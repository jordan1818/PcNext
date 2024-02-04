using Serilog.Core;
using Serilog.Events;

namespace Asys.Cli.Framework.Diagnostics.Logging.Serilog.Enrichers;

/// <summary>
/// Allows to enrich logs with information about <see cref="Exception"/>.
/// </summary>
public class ExceptionTypeEnricher : ILogEventEnricher
{
    /// <inheritdoc/>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (logEvent.Exception == null)
            return;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ExceptionType", logEvent.Exception.GetType().Name));
    }
}
