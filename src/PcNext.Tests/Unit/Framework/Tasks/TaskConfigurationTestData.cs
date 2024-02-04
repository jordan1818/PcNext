using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Asys.Tests.Framework.Data.Tasks;
using Asys.Tests.Framework.Logger;
using PcNext.Framework.Configurations;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal abstract class TaskConfigurationTestData<TTask, TTaskConfigurationTestData> : TaskTestData<TTask, TaskContext, TaskResults, TTaskConfigurationTestData>
        where TTask : ITask<TaskContext, TaskResults>
        where TTaskConfigurationTestData : TaskConfigurationTestData<TTask, TTaskConfigurationTestData>
{
    public TaskConfigurationTestData(ITestLogger logger)
    {
        Context = new TaskContext(logger);
        TaskConfiguration = new TaskConfiguration();
    }

    protected override TaskContext Context { get; set; }

    private TaskConfiguration TaskConfiguration { get; }

    protected abstract TTask CreateTask(TaskConfiguration taskConfiguration);

    protected override TTask CreateTask() => CreateTask(TaskConfiguration);

    public TTaskConfigurationTestData WithProperty(string name, string value)
    {
        TaskConfiguration.Properties[name] = value;
        return (TTaskConfigurationTestData)this;
    }
}
