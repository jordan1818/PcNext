using Asys.Tasks;
using PcNext.Framework.Configurations;

namespace PcNext.Framework;

public interface ITaskConfigurationTaskFactory
{
    ITask Create(TaskConfiguration taskConfiguration);
}
