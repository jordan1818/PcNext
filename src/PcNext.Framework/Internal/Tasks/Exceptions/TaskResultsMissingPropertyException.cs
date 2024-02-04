using Asys.Tasks;

namespace PcNext.Framework.Internal.Tasks.Exceptions;

internal sealed class TaskResultsMissingPropertyException : TaskResultsException
{
    public TaskResultsMissingPropertyException(string propertyName)
       : base($"Required property is missing '{propertyName}'.")
    {
    }
}
