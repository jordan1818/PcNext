namespace PcNext.Framework.Configurations;

internal static class TaskConfigurationExtensions
{
    public static string? GetPropertyValue(this TaskConfiguration taskConfiguration, string propertyName) => taskConfiguration.Properties.TryGetValue(propertyName, out var v) ? v : null;

    public static Uri? GetPropertyUriValue(this TaskConfiguration taskConfiguration, string name)
    {
        var value = taskConfiguration.GetPropertyValue(name);
        return Uri.TryCreate(value, new UriCreationOptions(), out var url) ? url : null;
    }

    public static T? GetPropertyEnumValue<T>(this TaskConfiguration taskConfiguration, string name)
       where T : struct, Enum
    {
        var value = taskConfiguration.GetPropertyValue(name);
        return Enum.TryParse<T>(value, ignoreCase: true, out var v) ? v : null;
    }

    public static bool GetPropertyBoolValue(this TaskConfiguration taskConfiguration, string name)
    {
        var value = taskConfiguration.GetPropertyValue(name);
        return bool.TryParse(value, out var v) && v;
    }
}
