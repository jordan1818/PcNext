namespace Asys.System.Convert;

/// <summary>
/// <see cref="Asys.System"/> version of convertion for <see cref="Enum"/> objects.
/// </summary>
public static class ConvertEnum
{
    /// <summary>
    /// Converts one <see cref="Enum"/> to another.
    /// </summary>
    /// <typeparam name="TCurrent">The current <see cref="Enum"/> to convert.</typeparam>
    /// <typeparam name="TOrigin">The origin <see cref="Enum"/> to convert too.</typeparam>
    /// <param name="current">The current value of <see cref="Enum"/> to convert.</param>
    /// <returns>Returns the origin <see cref="Enum"/>.</returns>
    /// <exception cref="InvalidCastException">Will throw if <paramref name="current"/> could not be converted.</exception>
    public static TOrigin? ToOrigin<TCurrent, TOrigin>(TCurrent? current)
        where TCurrent : struct, Enum
        where TOrigin : struct, Enum
    => current is not null ? Enum.TryParse<TOrigin>(Enum.GetName(current.Value), ignoreCase: false, out var origin) ? origin : throw new InvalidCastException($"Type of '{typeof(TCurrent).Name}' can not be converted to '{typeof(TOrigin).Name}'.") : null;
}
