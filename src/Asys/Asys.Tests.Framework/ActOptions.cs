namespace Asys.Tests.Framework;

/// <summary>
/// The definition of <see cref="ActOptions"/>
/// for instance of <see cref="ITestData{TActOptions}"/>.
/// </summary>
public class ActOptions
{
    /// <summary>
    /// The amount of times to internally execute 
    /// the <see cref="ITestData{TActOptions}.ActAsync(TActOptions?)"/> method.
    /// Default, it is 1.
    /// </summary>
    public uint ActTimes { get; set; } = 1;
}
