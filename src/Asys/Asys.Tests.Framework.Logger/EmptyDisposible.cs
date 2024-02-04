namespace Asys.Tests.Framework
{
    /// <summary>
    /// The object for empty <see cref="IDisposable"/>
    /// instances.
    /// </summary>
    internal sealed class EmptyDisposible : IDisposable
    {
        /// <summary>
        /// Initlaizes an instance of <see cref="EmptyDisposible"/>.
        /// </summary>
        private EmptyDisposible() { }

        /// <summary>
        /// Instantiates an instance of <see cref="EmptyDisposible"/>.
        /// </summary>
        /// <returns>The intance of <see cref="EmptyDisposible"/>.</returns>
        public static EmptyDisposible Create() => new ();

        /// <inheritdoc/>
        public void Dispose() { }
    }
}
