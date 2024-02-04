using Asys.System.IO;

namespace Asys.Cli.Extensions.Asys.System.IO;

/// <summary>
/// The definition of <see cref="IFileSystemFactory"/>.
/// </summary>
public interface IFileSystemFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IFileSystem"/>.
    /// </summary>
    /// <returns>An instance of a implemented <see cref="IFileSystem"/>.</returns>
    IFileSystem Create();
}
