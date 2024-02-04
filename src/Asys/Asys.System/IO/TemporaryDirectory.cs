namespace Asys.System.IO;

/// <summary>
/// The implementation of <see cref="ITemporaryDirectory"/>
/// within <see cref="TemporaryDirectory"/>
/// which allows to use a directory and remove after use.
/// </summary>
public sealed class TemporaryDirectory : ITemporaryDirectory
{
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// Initializes an instance of <see cref="TemporaryDirectory"/>.
    /// </summary>
    /// <param name="fileSystem">The instance of <see cref="IFileSystem"/> to delete the temporary path after use.</param>
    /// <param name="path">The full temporary directory path.</param>
    public TemporaryDirectory(IFileSystem fileSystem, string path)
    {
        _fileSystem = fileSystem;
        Path = path;
    }

    /// <inheritdoc/>
    public string Path { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        try
        {
            _fileSystem.DeleteDirectory(Path);
        }
        catch
        {
            // Ignore all exceptions
            // Should never fail here.
        }
    }
}
