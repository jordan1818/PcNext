using Asys.Tasks;

namespace PcNext.Framework.Internal.Tasks.Exceptions;

internal sealed class TaskResultsPropertyExpectationException : TaskResultsException
{
    public TaskResultsPropertyExpectationException(string propertyName, object? actualValue, object exampleValue)
       : base($"Invalid '{propertyName}'. Expected similar to {exampleValue}, but instead recieved '{actualValue}'.")
    {
    }
}