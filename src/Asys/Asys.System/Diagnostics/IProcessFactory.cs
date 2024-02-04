namespace Asys.System.Diagnostics;

/// <summary>
/// The definition of <see cref="IProcessFactory"/>
/// which allows to create an instance of <see cref="IProcess"/>
/// from an instance of <see cref="ProcessInformation"/>.
/// </summary>
public interface IProcessFactory
{
    /// <summary>
    /// Create an instance of <see cref="IProcess"/>
    /// from an instance of <see cref="ProcessInformation"/>.
    /// </summary>
    /// <param name="information">The <see cref="IProcess"/> information to execute with.</param>
    /// <returns>An implemented instance of <see cref="IProcess"/>.</returns>
    IProcess Create(ProcessInformation information);
}
