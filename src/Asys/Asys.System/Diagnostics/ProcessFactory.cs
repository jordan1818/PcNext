namespace Asys.System.Diagnostics;

/// <summary>
/// The implementation of <see cref="IProcessFactory"/> within <see cref="ProcessFactory"/>.
/// </summary>
public sealed class ProcessFactory : IProcessFactory
{
    /// <inheritdoc/>
    public IProcess Create(ProcessInformation information) => new Process(information);
}
