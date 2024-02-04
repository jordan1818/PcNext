namespace PcNext.Framework.Configurations;

public sealed class TaskConfiguration
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public TaskType Type { get; set; }

    public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
}
