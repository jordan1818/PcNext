namespace Asys.Tests.Framework.Asserts;

/// <summary>
/// The definition of <see cref="IAssert"/>
/// for general assertion sets.
/// </summary>
public interface IAssert : INullAssert, ITypeAssert, IEqualAssert, ICollectionAssert, IConditionAssert
{
}
