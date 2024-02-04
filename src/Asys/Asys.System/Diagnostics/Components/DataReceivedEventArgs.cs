using SystemDataRecievedEventArgs = System.Diagnostics.DataReceivedEventArgs;

namespace Asys.System.Diagnostics.Components;

/// <summary>
/// The event arguments recieved from <see cref="IProcess"/>.
/// Based off of <see cref="SystemDataRecievedEventArgs"/>
/// </summary>
public sealed class DataReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes an instance of <see cref="DataReceivedEventArgs"/>.
    /// </summary>
    /// <param name="data">The data</param>
    public DataReceivedEventArgs(string? data)
    {
        Data = data;
    }

    /// <summary>
    /// The data received from the <see cref="IProcess"/>.
    /// </summary>
    public string? Data { get; }
}
