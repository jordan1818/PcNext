using Asys.Tasks.Process;
using Asys.Tasks.Process.Context;
using Asys.Tasks.Process.Results;
using Asys.Tests.Framework.Data.Tasks;
using Asys.Tests.Framework.Logger;

namespace Asys.Tests.Intergration.Tasks.Process;

internal sealed class ProcessTaskTestData : TaskTestData<ProcessTask, ProcessTaskContext, ProcessTaskResults, ProcessTaskTestData>
{
    private readonly ITestLogger _logger;

    public ProcessTaskTestData(ITestLogger logger)
    {
        _logger = logger;
        Context = new ProcessTaskContext(_logger, $"{nameof(ProcessTaskTestData)}.exe");
    }


    protected override ProcessTaskContext Context { get; set; }

    protected override ProcessTask CreateTask() => new();

    public ProcessTaskTestData WithPowerShellContext(string arguments)
    {
        Context = new PowerShellProcessTaskContext(_logger)
        {
            Arguments = arguments,
        };

        return this;
    }

    public ProcessTaskTestData WithCmdContext(string arguments)
    {
        Context = new CmdProcessTaskContext(_logger)
        {
            Arguments = arguments,
        };

        return this;
    }
}
