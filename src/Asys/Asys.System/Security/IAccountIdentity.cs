namespace Asys.System.Security;

/// <summary>
/// The defintion of <see cref="IAccountIdentity"/>
/// which represents information for account information.
/// </summary>
public interface IAccountIdentity
{
    /// <summary>
    /// The id of the account.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// The name of the account.
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// The full name of the account.
    /// This may include, but not limited to,
    /// '<see cref="MachineName"/>\<see cref="Name"/>'
    /// or other alternatives. This may vary.
    /// </summary>
    string? FullName { get; }

    /// <summary>
    /// The machine name of the account is used on.
    /// </summary>
    string? MachineName { get; }
}
