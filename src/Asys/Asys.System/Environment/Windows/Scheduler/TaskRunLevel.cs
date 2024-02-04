using System.Xml.Serialization;

namespace Asys.System.Environment.Windows.Scheduler;

/// <summary>
/// Defines LUA elevation flags that specify with what privilege level the task will be run.
/// Base off of <see cref="Microsoft.Win32.TaskScheduler.TaskRunLevel"/>.
/// </summary>
public enum TaskRunLevel
{
    /// <summary>
    /// Tasks will be run with the least privileges.
    /// </summary>
    [XmlEnum("LeastPrivilege")]
    LUA,

    /// <summary>
    /// Tasks will be run with the highest privileges.
    /// </summary>
    [XmlEnum("HighestAvailable")]
    Highest,
}
