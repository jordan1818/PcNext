namespace Asys.Cli.Framework.Internal;

/// <summary>
/// An implementation of <see cref="IDisposable"/> allowing to compound multiple <see cref="IDisposable"/> together.
/// </summary>
public class CompositeDisposable : IDisposable
{
    private readonly IEnumerable<IDisposable?> _disposables;

    /// <summary>
    /// Initializes a new <see cref="CompositeDisposable"/> instance.
    /// </summary>
    /// <param name="disposables">The <see cref="IDisposable"/> instances to compound.</param>
    public CompositeDisposable(params IDisposable?[] disposables)
    {
        _disposables = disposables;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable?.Dispose();
        }
    }
}
