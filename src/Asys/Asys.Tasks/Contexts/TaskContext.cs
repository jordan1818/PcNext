using Asys.System.Components;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Asys.Tasks.Contexts;

/// <summary>
/// The definition of <see cref="TaskContext"/> for <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> to consume.
/// </summary>
public class TaskContext : INotifyPropertyChanged
{
    private TaskState _taskState = TaskState.Unknown;

    /// <summary>
    /// An event that will be raised for 
    /// any propeties that can be updated
    /// periodically.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes an instance of <see cref="TaskContext"/> for a single <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> use.
    /// </summary>
    /// <param name="logger">The logger to be used for the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.</param>
    /// <remarks>
    /// For more <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instances, it is recommended to create new instances of <see cref="TaskContext"/>.
    /// This will allow for 'clean' information to be provided to the other <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instances.
    /// </remarks>
    public TaskContext(ILogger logger)
    {
        Logger = logger;

        PropertyChanged += (s, e) =>
        {
            var propertyType = "N/A";
            var oldValue = "N/A";
            var newValue = "N/A";

            var extendedPropertyChangedEventArgs = e as ExtendedPropertyChangedEventArgs;
            if (extendedPropertyChangedEventArgs is not null)
            {
                propertyType = extendedPropertyChangedEventArgs.PropertyType?.Name ?? propertyType;
                oldValue = extendedPropertyChangedEventArgs.OldValue?.ToString() ?? oldValue;
                newValue = extendedPropertyChangedEventArgs.NewValue?.ToString() ?? newValue;
            }

            Logger.LogTrace("Setting '{Property}' property of type '{PropertyType}' from '{OldValue}' to '{NewValue}'.", e.PropertyName, propertyType, oldValue, newValue);
        };
    }

    /// <summary>
    /// The Logger for the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// The cancellation token for the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </summary>
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// The current state of the <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/>.
    /// </summary>
    /// <remarks>
    /// The value of this will be updated within <see cref="ITask"/> or <see cref="ITask{TTaskContext, TTaskContext}"/> instance.
    /// See <see cref="TaskState"/> for each state definition.
    /// </remarks>
    public TaskState State
    {
        get => _taskState;
        set
        {
            if (_taskState != value)
            {
                PropertyUpdated(nameof(State), State.GetType(), _taskState, value);
                _taskState = value;
            }
        }
    }

    protected void PropertyUpdated(string propertyName, Type? propertyType = null, object? oldValue = default, object? newValue = default) => PropertyUpdated(new ExtendedPropertyChangedEventArgs(propertyName, propertyType, oldValue, newValue));

    protected void PropertyUpdated(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
}
