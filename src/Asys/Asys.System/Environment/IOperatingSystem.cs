namespace Asys.System.Environment
{
    /// <summary>
    /// The definition of <see cref="IOperatingSystem"/>
    /// which represents an operating system information.
    /// </summary>
    public interface IOperatingSystem
    {
        /// <summary>
        /// Indicates wheather the current operating system is running Windows.
        /// </summary>
        /// <returns>True if the current operating system is running Windows; otherwise false.</returns>
        bool IsWindows();

        /// <summary>
        /// Indicates wheather the current operating system is running Linux.
        /// </summary>
        /// <returns>True if the current operating system is running Linux; otherwise false.</returns>
        bool IsLinux();

        /// <summary>
        /// Indicates wheather the current operating system is running Mac (OsX).
        /// </summary>
        /// <returns>True if the current operating system is running Mac (OsX); otherwise false.</returns>
        bool IsOsX();
    }
}
