namespace Asys.System.Environment.Windows.Scheduler.Triggers;

public interface ITaskTrigger
{
    /// <summary>
    /// A Boolean value that indicates whether the trigger is enabled.
    /// </summary>
    bool Enabled { get; set; }
}
