using Asys.System.Environment;
using Asys.System.Environment.Windows.Registry;
using Asys.Tasks;
using Asys.Tasks.Contexts;
using Asys.Tasks.Results;
using Microsoft.Extensions.Logging;
using PcNext.Framework.Configurations;
using PcNext.Framework.Internal.Tasks.Exceptions;
using System.Text.RegularExpressions;

namespace PcNext.Framework.Internal.Tasks;

internal sealed class RegistryTask : ITask
{
    private static readonly Regex _expanedStringRegex = new("%\\w+%", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    private readonly TaskConfiguration _taskConfiguration;
    private readonly IRegistry _registry;
    private readonly IOperatingSystem _operatingSystem;

    internal enum RegistryAction
    {
        Append,
        Update,
        Remove,
    }

    public RegistryTask(TaskConfiguration taskConfiguration, IRegistry registry, IOperatingSystem operatingSystem)
    {
        _taskConfiguration = taskConfiguration;
        _registry = registry;
        _operatingSystem = operatingSystem;

        Hive = taskConfiguration.GetPropertyEnumValue<RegistryHive>(nameof(Hive));
        Action = taskConfiguration.GetPropertyEnumValue<RegistryAction>(nameof(Action));
        Path = taskConfiguration.GetPropertyValue(nameof(Path));
        Key = taskConfiguration.GetPropertyValue(nameof(Key));
        Value = taskConfiguration.GetPropertyValue(nameof(Value));
    }

    internal RegistryHive? Hive { get; }

    internal RegistryAction? Action { get; }

    internal string? Path { get; }

    internal string? Key { get; }

    internal string? Value { get; }

    public Task<TaskResults> ExecuteAsync(TaskContext context)
    {
        return Task.Run(() =>
        {
            try
            {
                context.State = TaskState.Preparing;

                if (!_operatingSystem.IsWindows())
                {
                    throw new TaskResultsException($"The current operating is not running on Windows. Cannot run '{_taskConfiguration.Name}' task.");
                }

                if (Hive is null)
                {
                    throw new TaskResultsAcceptablePropertyValuesExceptions(nameof(Hive), Enum.GetNames<RegistryHive>());
                }

                if (Action is null)
                {
                    throw new TaskResultsAcceptablePropertyValuesExceptions(nameof(Action), Enum.GetNames<RegistryAction>());
                }

                if (string.IsNullOrWhiteSpace(Path))
                {
                    throw new TaskResultsMissingPropertyException(nameof(Path));
                }

                if (string.IsNullOrWhiteSpace(Key))
                {
                    throw new TaskResultsMissingPropertyException(nameof(Key));
                }

                using var registryKey = _registry.Get(Hive.Value)?.CreateOrOpenSubKey(Path, RegistryKeyPermissionCheck.ReadWriteSubTree) 
                    ?? throw new TaskResultsException($"Registry path '{Path}' within '{Hive}' could not be opened or created.");

                context.State = TaskState.Running;

                switch (Action)
                {

                    case RegistryAction.Update:
                        if (string.IsNullOrWhiteSpace(Value))
                        {
                            throw new TaskResultsMissingPropertyException(nameof(Value));
                        }

                        var valueKind = registryKey.GetValueKind(Key)
                            ?? GetValueKindFromValue(GetTypedValue(Value));

                        context.Logger.LogDebug("Creating/Updating Registry key '{RegistryKey}' with value '{RegistryValue}' of kind '{RegistryKind}'...", Key, Value, valueKind);
                        registryKey!.SetValue(Key, Value, valueKind);
                        break;

                    case RegistryAction.Append:
                        if (string.IsNullOrWhiteSpace(Value))
                        {
                            throw new TaskResultsMissingPropertyException(nameof(Value));
                        }

                        var typedValue = GetTypedValue(Value);
                        var value = registryKey.GetValue(Key) + typedValue ?? typedValue;
                        var valueKind = registryKey.GetValueKind(Key)
                            ?? GetValueKindFromValue(typedValue);

                        context.Logger.LogDebug("Creating/Appending Registry key '{RegistryKey}' with value '{RegistryValue}' of kind '{RegistryKind}'...", Key, value, valueKind);
                        registryKey!.SetValue(Key, Value, valueKind);
                        break;
                    case RegistryAction.Remove:
                        context.Logger.LogDebug("Removing Registry key '{RegistryKey}'", Key);
                        registryKey!.DeleteValue(Key);
                        break;
                    default:
                        throw new NotSupportedException($"'{nameof(Action)}' is not support of value '{Action}'.");
                }
            }
            catch (TaskResultsException e)
            {
                return TaskResults.Failure(e.Message, e.InnerException);
            }
            catch (Exception e)
            {
                return TaskResults.Failure($"An unhandled exception occurred when {Action!.ToString()!.ToLowerInvariant()}ing registry key '{Key}' from '{Hive}\\{Path}' with value '{Value}'.", e);
            }
            finally
            {
                context.State = TaskState.Completed;
            }

            return TaskResults.Success();
        });
    }

    internal static object GetTypedValue(string value)
    {
        if (int.TryParse(value, out var i))
        {
            return i;
        }

        if (long.TryParse(value, out var l))
        {
            return l;
        }

        var array1 = value.Split(",");
        var array2 = value.Split(";");
        if (array1.Any())
        {
            return array1;
        }
        else if (array1.Any())
        {
            return array2;
        }

        return value;
    }

    internal static RegistryValueKind GetValueKindFromValue(object value)
    {
        switch (value)
        {
            case int: return RegistryValueKind.DWord;
            case long: return RegistryValueKind.QWord;
            case IEnumerable<string>: return RegistryValueKind.MultiString;
            case string:
                {
                    if (_expanedStringRegex.IsMatch((string)value!))
                    {
                        return RegistryValueKind.ExpandString;
                    }

                    return RegistryValueKind.String;
                }
            default: return RegistryValueKind.String;
        }
    }
}
