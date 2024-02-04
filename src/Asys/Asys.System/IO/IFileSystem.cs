namespace Asys.System.IO;

/// <summary>
/// A definition of <see cref="IFileSystem"/> for file system operations.
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Determines if the file path given exists on the file system.
    /// </summary>
    /// <param name="filePath">The file path to validate.</param>
    /// <returns>True if the file path exists; otherwise false</returns>
    bool FileExists(string filePath);

    /// <summary>
    /// Determines if the directory path given exists on the file system.
    /// </summary>
    /// <param name="directoryPath">The directory path to validate.</param>
    /// <returns>True if the directory path exists; otherwise false</returns>
    bool DirectoryExists(string directoryPath);

    /// <summary>
    /// Deletes a file from the file system.
    /// </summary>
    /// <param name="filePath">The file path to delete.</param>
    void DeleteFile(string filePath);

    /// <summary>
    /// Creates the directory path.
    /// </summary>
    /// <param name="path">The directory path to create.</param>
    void CreateDirectory(string path);

    /// <summary>
    /// Deletes the directory path.
    /// </summary>
    /// <param name="path">The directory path to delete.</param>
    void DeleteDirectory(string path);

    /// <summary>
    /// Creates a temporary directory can be disposed of after.
    /// </summary>
    /// <param name="basePath">The base path for the temporary directory. If null, will have a default value.</param>
    /// <returns>Returns an instance of <see cref="ITemporaryDirectory"/>.</returns>
    ITemporaryDirectory CreateTemporaryDirectory(string? basePath = null);

    /// <summary>
    /// Creates or opens a file then writes to the file with the content and closes synchronously
    /// </summary>
    /// <param name="filePath">The path and name of the file to be written too.</param>
    /// <param name="content">The content in which the file will be appended with.</param>
    void AppendFile(string filePath, string? content);

    /// <summary>
    /// Creates or opens a file then writes to the file with the content and closes asynchronously
    /// </summary>
    /// <param name="filePath">The path and name of the file to be written too.</param>
    /// <param name="content">The content in which the file will be appended with.</param>
    /// <param name="cancellationToken">The cancellation token for the file to be written too.</param>
    Task AppendFileAsync(string filePath, string? content, CancellationToken cancellationToken = default);
}