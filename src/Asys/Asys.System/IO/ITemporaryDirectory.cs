namespace Asys.System.IO;

/// <summary>
/// The definition of <see cref="ITemporaryDirectory"/>
/// which allows to use a directory and remove after use.
/// </summary>
public interface ITemporaryDirectory : IDisposable
{
    /// <summary>
    /// The full path to the temporary directory.
    /// </summary>
    string Path { get; }
}
