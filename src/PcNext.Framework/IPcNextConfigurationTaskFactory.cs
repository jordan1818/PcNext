using Asys.Tasks;
using PcNext.Framework.Configurations;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PcNext.Tests")]

namespace PcNext.Framework;

public interface IPcNextConfigurationTaskFactory
{
    IEnumerable<ITask> Create(PcNextConfiguration pcNextConfiguration);
}
