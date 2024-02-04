using Asys.Tasks;
using PcNext.Framework.Configurations;

namespace PcNext.Framework.Internal;

public interface IChocolateyConfigurationTaskFactory
{
    ITask Create(ChocolateyConfiguration configuration);
}
