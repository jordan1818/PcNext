using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Asys.Tests.Framework.Data.Tasks;
using Asys.Tests.Framework.Logger;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal sealed class ShellTaskTestData : TaskTestData<ShellTask, TaskContext, TaskResults, ShellTaskTestData>
{
    public ShellTaskTestData(ITestLogger logger)
    {
        Context = new TaskContext(logger);
        TaskConfiguration = new TaskConfiguration();
    }

    protected override TaskContext Context { get; set; }

    private TaskConfiguration TaskConfiguration { get; }

    protected override ShellTask CreateTask()
    {
        throw new NotImplementedException();
    }
}
