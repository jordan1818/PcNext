using System.Globalization;

namespace Asys.System.IO;

/// <summary>
/// A implmentation of <see cref="IFileSystem"/> within <see cref="FileSystem"/> for file system operations.
/// </summary>
public sealed class FileSystem : IFileSystem
{

    /// <inheritdoc/>
    public bool FileExists(string filePath) => File.Exists(filePath);

    /// <inheritdoc/>
    public bool DirectoryExists(string directoryPath) => Directory.Exists(directoryPath);

    /// <inheritdoc/>
    public void DeleteFile(string filePath)
    {
        File.Delete(filePath);
    }

    /// <inheritdoc/>
    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    /// <inheritdoc/>
    public void DeleteDirectory(string path)
    {
        Directory.Delete(path);
    }

    /// <inheritdoc/>
    public ITemporaryDirectory CreateTemporaryDirectory(string? basePath = null)
    {
        var folderName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString("N");
        basePath = !string.IsNullOrWhiteSpace(basePath) ? basePath : Path.GetTempPath();

        return new TemporaryDirectory(this, Path.Combine(basePath, folderName));
    }

    /// <inheritdoc/>
    public void AppendFile(string filePath, string? content)
    {
        File.AppendAllText(filePath, content);
    }

    /// <inheritdoc/>
    public Task AppendFileAsync(string filePath, string? content, CancellationToken cancellationToken = default) => File.AppendAllTextAsync(filePath, content, cancellationToken);
}
