using SystemHttpMessageHandler = System.Net.Http.HttpMessageHandler;

namespace Asys.System.Net.Http;

/// <summary>
/// The implementation of <see cref="HttpMessageHandler"/>
/// for Asys.
/// </summary>
public sealed class HttpMessageHandler : SystemHttpMessageHandler
{
    private readonly IHttpMessageHandler _httpMessageHandler;

    /// <summary>
    /// Initializes an instance of <see cref="HttpMessageHandler"/>.
    /// </summary>
    /// <param name="httpMessageHandler">The implemented instance of <see cref="IHttpMessageHandler"/>.</param>
    public HttpMessageHandler(IHttpMessageHandler httpMessageHandler)
    {
        _httpMessageHandler = httpMessageHandler;
    }

    /// <inheritdoc/>
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) => SendAsync(request, cancellationToken).GetAwaiter().GetResult();

    /// <inheritdoc/>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => _httpMessageHandler.SendAsync(request, cancellationToken);

    /// <inheritdoc/>
    protected override void Dispose(bool disposing) => _httpMessageHandler.Dispose();
}
