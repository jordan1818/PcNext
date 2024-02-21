using Asys.System.Environment.Windows.Registry;
using Xunit;
using Assert = Asys.Tests.Framework.Asserts.Assert;

namespace Asys.Tests.Unit.System.Environment.Windows.Registry;

public sealed class RegistryValueTests
{
    public static IEnumerable<object[]> AppendValueSuccessfullyTestData => new List<object[]>
    {
        new object[] { new RegistryValue("1", RegistryValueKind.String),  new RegistryValue("2", RegistryValueKind.String), new RegistryValue("12", RegistryValueKind.String)},
        new object[] { new RegistryValue("1", RegistryValueKind.ExpandString),  new RegistryValue("2", RegistryValueKind.ExpandString), new RegistryValue("12", RegistryValueKind.ExpandString) },
        new object[] { new RegistryValue((long)1, RegistryValueKind.QWord),  new RegistryValue((long)2, RegistryValueKind.QWord), new RegistryValue((long)3, RegistryValueKind.QWord)},
        new object[] { new RegistryValue(1, RegistryValueKind.DWord),  new RegistryValue(2, RegistryValueKind.DWord), new RegistryValue(3, RegistryValueKind.DWord) },
        new object[] { new RegistryValue(new[] { "1", "2" }, RegistryValueKind.MultiString),  new RegistryValue(new[] { "3", "4" }, RegistryValueKind.MultiString), new RegistryValue(new[] { "1", "2", "3", "4" }, RegistryValueKind.MultiString) },
    };

    [Theory]
    [MemberData(nameof(AppendValueSuccessfullyTestData))]
    public void Append_Value_Successfully(RegistryValue registryValue1, RegistryValue registryValue2, RegistryValue expectedRegistryValue)
    {
        // -- Act --
        var actualRegistryValue = registryValue1 + registryValue2;

        // -- Assert --
        Assert.NotNull(actualRegistryValue);
        Assert.Equal(expectedRegistryValue, actualRegistryValue);
    }

    [Theory]
    [InlineData(RegistryValueKind.None)]
    [InlineData(RegistryValueKind.Unknown)]
    [InlineData(RegistryValueKind.Binary)]
    public void Append_Value_Failed_With_Unsupported_Kind(RegistryValueKind kind)
    {
        // -- Act / Assert --
    }
}
