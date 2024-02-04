using SystemOperatingSystem = System.OperatingSystem;

namespace Asys.System.Environment;

/// <summary>
/// The implementation of <see cref="IOperatingSystem"/> within <see cref="OperatingSystem"/>.
/// Based off of <see cref="SystemOperatingSystem"/>.
/// </summary>
public sealed class OperatingSystem : IOperatingSystem
{
    /// <inheritdoc/>
    public bool IsWindows() => SystemOperatingSystem.IsWindows();

    /// <inheritdoc/>
    public bool IsLinux() => SystemOperatingSystem.IsLinux();

    /// <inheritdoc/>
    public bool IsOsX() => SystemOperatingSystem.IsMacCatalyst();
}
