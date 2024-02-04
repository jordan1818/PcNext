namespace Asys.Tests.Framework;

/// <summary>
/// The definition of <see cref="ITestData"/>
/// which allows to define a implentation 
/// test data for test and use a
/// fluent format.
/// Extends <see cref="ITestData{TActOptions}"/>.
/// </summary>
public interface ITestData : ITestData<ActOptions>
{
}
