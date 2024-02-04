using SystemDataReceivedEventHandler = System.Diagnostics.DataReceivedEventHandler;

namespace Asys.System.Diagnostics.Components;

/// <summary>
/// The event handler to receive data from <see cref="IProcess"/>.
/// Based off of <see cref="SystemDataReceivedEventHandler"/>
/// </summary>
/// <param name="sender">The sender of the data, an instance of <see cref="IProcess"/>.</param>
/// <param name="dataRecievedEventArgs">The instance of <see cref="DataReceivedEventArgs"/> with <see cref="IProcess"/> data.</param>
public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs dataRecievedEventArgs);
