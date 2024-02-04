namespace Asys.Tests.Framework;

/// <summary>
/// The definition of <see cref="ITestData{TActOptions}"/>
/// which allows to define a implentation 
/// test data for test and use a
/// fluent format.
/// </summary>
/// <typeparam name="TActOptions">The template of <see cref="ActOptions"/> for <see cref="ITestData{TActOptions}.ActAsync(TActOptions?)"/> options.</typeparam>
public interface ITestData<TActOptions> : IDisposable
    where TActOptions : ActOptions
{
    /// <summary>
    /// The test data to act upon asynchronously.
    /// </summary>
    /// <param name="actOptions">The instance of <see cref="TActOptions"/> for acting upon test data.</param>
    Task ActAsync(TActOptions? actOptions = null);
}
