namespace PcNext.Framework.Configurations;

public sealed class PcNextConfiguration
{
    public IEnumerable<ChocolateyConfiguration> Chocolatey { get; set; } = Enumerable.Empty<ChocolateyConfiguration>();

    public IEnumerable<TaskConfiguration> AdditionalTasks { get; set; } = Enumerable.Empty<TaskConfiguration>();
}
