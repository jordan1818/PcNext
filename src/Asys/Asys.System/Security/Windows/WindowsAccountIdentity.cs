using Asys.System.Security;
using WindowsIdentity = System.Security.Principal.WindowsIdentity;

namespace Asys.System.Security.Windows;

/// <summary>
/// The implementation of <see cref="IAccountIdentity"/> within <see cref="WindowsAccountIdentity"/>.
/// </summary>
public sealed class WindowsAccountIdentity : IAccountIdentity
{
    /// <summary>
    /// Initalizes an instance of <see cref="WindowsAccountIdentity"/>.
    /// </summary>
    public WindowsAccountIdentity()
    {
        var windowsIdentity = WindowsIdentity.GetCurrent();
        var fullName = windowsIdentity.Name;
        var splitFullName = fullName.Split('\\').AsEnumerable();

        Id = windowsIdentity.User?.Value;
        Name = splitFullName.ElementAt(1);
        FullName = windowsIdentity.Name;
        MachineName = splitFullName.ElementAt(0);
    }

    /// <inheritdoc/>
    public string? Id { get; }

    /// <inheritdoc/>
    public string? Name { get; }

    /// <inheritdoc/>
    public string? FullName { get; }

    /// <inheritdoc/>
    public string? MachineName { get; }
}
