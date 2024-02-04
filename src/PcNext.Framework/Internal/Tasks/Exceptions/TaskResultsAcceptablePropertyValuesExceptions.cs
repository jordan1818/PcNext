using Asys.Tasks;

namespace PcNext.Framework.Internal.Tasks.Exceptions;

internal sealed class TaskResultsAcceptablePropertyValuesExceptions : TaskResultsException
{
    public TaskResultsAcceptablePropertyValuesExceptions(string propertyName, string[] acceptablesValues)
        : base($"'{propertyName}' is missing or not a correct value. The acceptables values are '{string.Join(", ", acceptablesValues)}'.")
    {
    }
}
