namespace Asys.Cli.Framework.Internal;

/// <summary>
/// An implementation of <see cref="IDisposable"/> allowing a method to be called on <see cref="IDisposable.Dispose"/>.
/// </summary>
public class InlineDisposable : IDisposable
{
    private readonly Action _onDispose;

    /// <summary>
    /// Initializes a new <see cref="InlineDisposable"/> instance.
    /// </summary>
    /// <param name="onDispose">The method to call when <see cref="IDisposable.Dispose"/> is called.</param>
    public InlineDisposable(Action onDispose)
    {
        _onDispose = onDispose;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _onDispose.Invoke();
        GC.SuppressFinalize(this);
    }
}
