using Asys.System.Environment;
using Asys.System.Environment.Windows.Registry;
using Asys.Mocks.System.Environment;
using Asys.Tests.Framework.Logger;
using Moq;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks;
using Asys.Mocks.System.Environment.Windows.Registry;
using static PcNext.Framework.Internal.Tasks.RegistryTask;

namespace PcNext.Tests.Unit.Framework.Tasks;

internal sealed class RegistryTaskTestData : TaskConfigurationTestData<RegistryTask, RegistryTaskTestData>
{
    public RegistryTaskTestData(ITestLogger logger)
        : base(logger)
    {
        MockRegistry = new Mock<IRegistry>();
        MockRegistryKey = new Mock<IRegistryKey>();
        MockOperatingSystem = new Mock<IOperatingSystem>();

        MockRegistry.ReturnOnGet(() => MockRegistryKey.Object);

        // Default behaviour.
        WithValidSetup();
    }

    private Mock<IRegistry> MockRegistry { get; }

    private Mock<IRegistryKey> MockRegistryKey { get; }

    private Mock<IOperatingSystem> MockOperatingSystem { get; }

    protected override RegistryTask CreateTask(TaskConfiguration taskConfiguration) => new(taskConfiguration, MockRegistry.Object, MockOperatingSystem.Object);

    public RegistryTaskTestData WithValidSetup()
        => WithOsIsWindows()
        .WithHive(RegistryHive.LocalMachine.ToString())
        .WithAction(RegistryAction.Append.ToString())
        .WithPath("X\\Y\\Z")
        .WithKey("MockKey")
        .WithValue("MockValue");

    public RegistryTaskTestData WithHive(string hive)
        => WithProperty(nameof(RegistryTask.Hive), hive);

    public RegistryTaskTestData WithAction(string action)
        => WithProperty(nameof(RegistryTask.Action), action);

    public RegistryTaskTestData WithPath(string path)
        => WithProperty(nameof(RegistryTask.Path), path);

    public RegistryTaskTestData WithKey(string key)
        => WithProperty(nameof(RegistryTask.Key), key);

    public RegistryTaskTestData WithValue(string value)
        => WithProperty(nameof(RegistryTask.Value), value);

    public RegistryTaskTestData WithOsIsWindows()
    {
        MockOperatingSystem.OnIsWindows(true);
        return this;
    }

    public RegistryTaskTestData WithOsIsNotWindows()
    {
        MockOperatingSystem.OnIsWindows(false);
        return this;
    }

    public RegistryTaskTestData ReturnNullOnRegistryHiveRetrieval()
    {
        MockRegistry.ReturnOnGet(result: null);
        return this;
    }

    public RegistryTaskTestData ThrowOnRegistryHiveRetrieval<TException>()
        where TException : Exception, new()
    {
        MockRegistry.ThrowsOnGet<TException>();
        return this;
    }

    public RegistryTaskTestData VerifyRegistryInteractions()
    {

        return this;
    }

    public RegistryTaskTestData VerifyNoRegistryInteractions()
    {
        MockRegistry.VerifyOnGet(Times.Never);
        return this;
    }

    public RegistryTaskTestData VerifyNoRegistryUpdates()
    {
        MockRegistryKey
        return this;
    }
}
