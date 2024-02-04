using Microsoft.Extensions.Logging;

namespace Asys.Tests.Framework.Logger;

/// <summary>
/// The expansion of <see cref="TestLogger"/>
/// with <see cref="ITestLogger{TCategory}"/>
/// implmenentation.
/// </summary>
/// <typeparam name="TCategory">The logger category.</typeparam>
public class TestLogger<TCategory> : TestLogger, ITestLogger<TCategory>
{
    /// <summary>
    /// Inializes an instane of <see cref="TestLogger{T}"/>.
    /// </summary>
    /// <param name="allowedLevel">The allowed <see cref="LogLevel"/> to log up to.</param>
    public TestLogger(LogLevel allowedLevel = LogLevel.Trace)
        : base(allowedLevel)
    {

    }
}
