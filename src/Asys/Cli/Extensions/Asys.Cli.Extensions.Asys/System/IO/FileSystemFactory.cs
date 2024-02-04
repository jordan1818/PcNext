using Asys.System.IO;

namespace Asys.Cli.Extensions.Asys.System.IO;

/// <summary>
/// The implementation of <see cref="IFileSystemFactory"/> within <see cref="FileSystemFactory"/>.
/// </summary>
public sealed class FileSystemFactory : IFileSystemFactory
{
    private readonly IFileSystem? _fileSystem;

    /// <summary>
    /// Initializes an instance of <see cref="FileSystemFactory"/>.
    /// </summary>
    /// <param name="fileSystem">An instance of <see cref="IFileSystem"/> to use instead of creating a new instance. Can be null.</param>
    public FileSystemFactory(IFileSystem? fileSystem)
    {
        _fileSystem = fileSystem;
    }

    /// <inheritdoc/>
    public IFileSystem Create() => _fileSystem ?? new FileSystem();
}
