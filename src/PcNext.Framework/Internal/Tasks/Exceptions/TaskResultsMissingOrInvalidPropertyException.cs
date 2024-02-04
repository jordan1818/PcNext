using Asys.Tasks;

namespace PcNext.Framework.Internal.Tasks.Exceptions;

internal sealed class TaskResultsMissingOrInvalidPropertyException : TaskResultsException
{
    public TaskResultsMissingOrInvalidPropertyException(string propertyName, object? propertyValue)
       : base($"Required property is missing '{propertyName}' or is invalid. Value: 'propertyValue'.")
    {
    }
}
