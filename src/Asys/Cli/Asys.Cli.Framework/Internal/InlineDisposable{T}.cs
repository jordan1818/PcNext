namespace Asys.Cli.Framework.Internal;

/// <summary>
/// An implementation of <see cref="IDisposable"/> allowing a method to be called on <see cref="IDisposable.Dispose"/>.
/// </summary>
public class InlineDisposable<T> : InlineDisposable
{
    /// <summary>
    /// The inner object relevant to this <see cref="IDisposable"/>.
    /// </summary>
    public T Inner { get; set; }

    /// <summary>
    /// Initializes a new <see cref="InlineDisposable{T}"/> instance.
    /// </summary>
    /// <param name="inner">The inner object relevant to this <see cref="IDisposable"/>.</param>
    /// <param name="onDispose">The method to call when <see cref="IDisposable.Dispose"/> is called.</param>
    public InlineDisposable(T inner, Action onDispose)
        : base(onDispose)
    {
        Inner = inner;
    }
}
