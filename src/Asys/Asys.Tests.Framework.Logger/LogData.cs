using Microsoft.Extensions.Logging;

namespace Asys.Tests.Framework.Logger;

/// <summary>
/// A POCO object for <see cref="TestLogger"/> and <see cref="TestLogger{TCategory}"/>
/// to contain all log event items.
/// </summary>
public sealed class LogData
{
    /// <summary>
    /// Initlaizes an instance of <see cref="LogData"/>.
    /// </summary>
    /// <param name="level">The level of the log event.</param>
    /// <param name="id">The id of the log event</param>
    /// <param name="message">The message of the log event.</param>
    /// <param name="exception">The exception of the log event.</param>
    public LogData(LogLevel level, EventId id, string? message, Exception? exception)
    {
        Level = level;
        Id = id;
        Message = message;
        Exception = exception;
    }

    /// <summary>
    /// The level of the log event.
    /// </summary>
    public LogLevel Level { get; }

    /// <summary>
    /// The id of the log event.
    /// </summary>
    public EventId Id { get; }

    /// <summary>
    /// The message of the log event.
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// The exception of the log event.
    /// </summary>
    public Exception? Exception { get; }
}
