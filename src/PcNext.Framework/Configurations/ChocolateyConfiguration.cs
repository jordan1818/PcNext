namespace PcNext.Framework.Configurations;

public sealed class ChocolateyConfiguration
{
    public ChocolateyConfiguration()
    {
        Name = string.Empty;
    }

    public string Name { get; set; }

    public string? Version { get; set; }

    public string? PackageParameters { get; set; }

    public IList<TaskConfiguration> BeforeTasks { get; set; } = Enumerable.Empty<TaskConfiguration>().ToList();

    public IList<TaskConfiguration> AfterTasks { get; set; } = Enumerable.Empty<TaskConfiguration>().ToList();
}
