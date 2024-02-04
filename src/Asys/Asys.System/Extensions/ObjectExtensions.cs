using System.Xml.Serialization;

namespace Asys.System.Extensions;

/// <summary>
/// The extension for any <see cref="object"/> instances.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts full <see cref="object"/> to Xml string.
    /// </summary>
    /// <param name="object">The instance of <see cref="object"/> operate on.</param>
    /// <returns>The Xml string format of the <see cref="object"/> instance.</returns>
    public static string ToXmlString(this object @object)
    {
        using var stringwriter = new StringWriter();
        var serializer = new XmlSerializer(@object.GetType());

        serializer.Serialize(stringwriter, @object);

        return stringwriter.ToString();
    }
}
