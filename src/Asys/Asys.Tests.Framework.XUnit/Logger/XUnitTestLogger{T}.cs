using Asys.Tests.Framework.Logger;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Asys.Tests.Framework.XUnit.Logger;

/// <summary>
/// The expansion of <see cref="TestLogger{TCategory}"/> for XUnit
/// with <see cref="ITestLogger{TCategory}"/>
/// implmenentation.
/// </summary>
/// <typeparam name="TCategory">The logger category.</typeparam>
public class XUnitTestLogger<TCategory> : TestLogger<TCategory>, ITestLogger<TCategory>
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
