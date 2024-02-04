namespace Asys.System.Security;

/// <summary>
/// The definition of <see cref="IAccountManager"/>
/// which retrives <see cref="IAccountIdentity"/> information.
/// </summary>
public interface IAccountManager
{
    /// <summary>
    /// Retrieves the current <see cref="IAccountIdentity"/> information.
    /// </summary>
    /// <returns>
    /// Return an instance of <see cref="IAccountIdentity"/>, 
    /// but if it cannot detemine the current <see cref="IAccountIdentity"/>,
    /// this will return <b>null</b>.
    /// </returns>
    IAccountIdentity? GetCurrentIdentity();
}
