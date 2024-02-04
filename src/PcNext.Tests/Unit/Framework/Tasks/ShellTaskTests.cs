using Asys.Tests.Framework.Logger;
using Asys.Tests.Framework.XUnit.Logger;
using Xunit.Abstractions;

namespace PcNext.Tests.Unit.Framework.Tasks;

public sealed class ShellTaskTests
{
    private readonly ITestLogger _logger;

    public ShellTaskTests(ITestOutputHelper output)
    {
        _logger = new XUnitTestLogger(output);
    }
}
