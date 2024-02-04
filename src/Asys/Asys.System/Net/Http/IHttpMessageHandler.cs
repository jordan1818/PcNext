namespace Asys.System.Net.Http;

/// <summary>
/// The definition of <see cref="IHttpMessageHandler"/>
/// to be used within <see cref="AsysHttpMessageHandler"/>.
/// </summary>
public interface IHttpMessageHandler : IDisposable
{
    /// <summary>
    /// Sends an Http request asynchronously.
    /// </summary>
    /// <param name="request">The instance of <see cref="HttpRequestMessage"/> which contains the information to send.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronously Http request.</param>
    /// <returns>The instance of <see cref="HttpResponseMessage"/> which contains information from the received from the Http request.</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}
