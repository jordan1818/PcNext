namespace Asys.Tests.Framework.XUnit.Asserts;

using Assert = Xunit.Assert;

/// <summary>
/// The implementation of <see cref="Asys.Tests.Framework.Asserts.IThrowAssert"/>
/// for XUnit assertion sets.
/// </summary>
public sealed partial class XUnitAssert
{
    /// <inheritdoc/>
    public void Throws(Type exceptionType, Action action) => Assert.Throws(exceptionType, action);

    /// <inheritdoc/>
    public void Throws<TException>(Action action) where TException : Exception => Assert.Throws<TException>(action);

    /// <inheritdoc/>
    public Task ThrowsAsync(Type exceptionType, Func<Task> action)  => Assert.ThrowsAsync(exceptionType, action);

    /// <inheritdoc/>
    public Task ThrowsAsync<TException>(Func<Task> action) where TException : Exception => Assert.ThrowsAsync<TException>(action);
}
