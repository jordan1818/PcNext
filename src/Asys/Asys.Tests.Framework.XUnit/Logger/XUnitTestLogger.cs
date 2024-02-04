using Asys.Tests.Framework.Logger;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Asys.Tests.Framework.XUnit.Logger;

/// <summary>
/// The expansion of <see cref="TestLogger"/> for XUnit
/// with <see cref="ITestLogger"/>
/// implmenentation.
/// </summary>
public class XUnitTestLogger : TestLogger, ITestLogger
{
    /// <summary>
    /// Inializes an instane of <see cref="XUnitTestLogger"/>.
    /// </summary>
    /// <param name="output">The implemented instance of <see cref="ITestOutputHelper"/> for XUnit logging.</param>
    /// <param name="allowedLevel">The allowed <see cref="LogLevel"/> to log up to.</param>
    public XUnitTestLogger(ITestOutputHelper output, LogLevel allowedLevel = LogLevel.Trace)
        : base(allowedLevel)
    {
        OnLog += d => output.WriteLine(d.Message);
    }
}
