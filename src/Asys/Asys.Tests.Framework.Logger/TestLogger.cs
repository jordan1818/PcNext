using Microsoft.Extensions.Logging;

namespace Asys.Tests.Framework.Logger;

/// <summary>
/// The implementation of <see cref="ITestLogger"/> 
/// for test purposes.
/// </summary>
public class TestLogger : ITestLogger
{
    private readonly LogLevel _allowedLevel;
    private readonly IList<LogData> _logsData = new List<LogData>();

    /// <summary>
    /// The event handler for on log events.
    /// </summary>
    public event Action<LogData>? OnLog;

    /// <summary>
    /// Inializes an instane of <see cref="TestLogger"/>.
    /// </summary>
    /// <param name="allowedLevel">The allowed <see cref="LogLevel"/> to log up to.</param>
    public TestLogger(LogLevel allowedLevel = LogLevel.Trace)
    {
        _allowedLevel = allowedLevel;
        _logsData = new List<LogData>();
    }

    /// <summary>
    /// The collection of <see cref="LogData"/>.
    /// </summary>
    public IReadOnlyList<LogData> LogsData => _logsData.ToList();

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => EmptyDisposible.Create();

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => logLevel >= _allowedLevel;

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        var logData = new LogData(logLevel, eventId, message, exception);

        // Store the log event within a in memory list.
        _logsData.Add(logData);

        // Fire off an event for this log event.
        OnLog?.Invoke(logData);
    }
}
