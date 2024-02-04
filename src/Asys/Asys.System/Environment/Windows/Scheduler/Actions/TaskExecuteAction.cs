using System.Xml.Serialization;

namespace Asys.System.Environment.Windows.Scheduler.Actions;

/// <summary>
/// The definition of <see cref="TaskExecuteAction"/>
/// which represents an action that executes a command-line operation.
/// Based off of <see cref="Microsoft.Win32.TaskScheduler.ExecAction"/>.
/// </summary>
[XmlRoot("Exec")]
public sealed class TaskExecuteAction : ITaskAction
{
    /// <summary>
    /// Initializes an instance of <see cref="TaskExecuteAction"/>
    /// </summary>
    /// <param name="command">The command to execute on task.</param>
    /// <param name="argument">The arguments for <paramref name="command"/> to execute with.</param>
    /// <param name="workingDirectory">The working directory for <paramref name="command"/> to execute under.</param>
    public TaskExecuteAction(string command, string? argument = null, string? workingDirectory = null)
    {
        Command = command;
        Argument = argument;
        WorkingDirectory = workingDirectory;
    }

    /// <summary>
    /// The command to execute on task.
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// The arguments for <see cref="Command"/> to execute with.
    /// </summary>
    public string? Argument { get; }

    /// <summary>
    /// The working directory for <see cref="Command"/> to execute under.
    /// </summary>
    public string? WorkingDirectory { get; }
}
