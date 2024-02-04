using Asys.System.Net.Http;
using Moq;
using System.Net;

namespace Asys.Mocks.System.Net.Http;

/// <summary>
/// Extensions for fluent setup of a <see cref="Mock{IHttpMessageHandler}"/> instance.
/// </summary>
public static class MockHttpMessageHandlerExtensions
{
    /// <summary>
    /// Setups mocked <see cref="IHttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/> to return a result.
    /// </summary>
    /// <param name="mockHttpMessageHandler">The instance of the <see cref="Mock{IHttpMessageHandler}"/> to configure.</param>
    /// <param name="httpResponseMessage">The <see cref="HttpResponseMessage"/> instance that will be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IHttpMessageHandler}"/> to allow for chainning.</returns>
    public static Mock<IHttpMessageHandler> OnSendAsyncReturns(this Mock<IHttpMessageHandler> mockHttpMessageHandler, HttpResponseMessage httpResponseMessage)
    {
        mockHttpMessageHandler.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponseMessage);
        return mockHttpMessageHandler;
    }

    /// <summary>
    /// Setups mocked <see cref="IHttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/> to return a result.
    /// </summary>
    /// <param name="mockHttpMessageHandler">The instance of the <see cref="Mock{IHttpMessageHandler}"/> to configure.</param>
    /// <param name="httpStatusCode">The <see cref="HttpResponseMessage"/> instance that will be returned.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IHttpMessageHandler}"/> to allow for chainning.</returns>
    public static Mock<IHttpMessageHandler> OnSendAsyncReturns(this Mock<IHttpMessageHandler> mockHttpMessageHandler, HttpStatusCode httpStatusCode)
        => mockHttpMessageHandler.OnSendAsyncReturns(new HttpResponseMessage(httpStatusCode));

    /// <summary>
    /// Setups a throw on a mocked <see cref="IHttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/>.
    /// </summary>
    /// <param name="mockHttpMessageHandler">The instance of the <see cref="Mock{IHttpMessageHandler}"/> to configure.</param>
    /// <param name="exception">The <see cref="Exception"/> instance that will be thrown.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IHttpMessageHandler}"/> to allow for chainning.</returns>
    public static Mock<IHttpMessageHandler> ThrowsOnSendAsync(this Mock<IHttpMessageHandler> mockHttpMessageHandler, Exception exception)
    {
        mockHttpMessageHandler.Setup(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Throws(exception);
        return mockHttpMessageHandler;
    }

    /// <summary>
    /// Setups a throw on a mocked <see cref="IHttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/>.
    /// </summary>
    /// <param name="mockHttpMessageHandler">The instance of the <see cref="Mock{IHttpMessageHandler}"/> to configure.</param>
    /// <returns>The confgiured instane of <see cref="Mock{IHttpMessageHandler}"/> to allow for chainning.</returns>
    public static Mock<IHttpMessageHandler> ThrowsOnSendAsync<TException>(this Mock<IHttpMessageHandler> mockHttpMessageHandler)
        where TException : Exception, new()
    => mockHttpMessageHandler.ThrowsOnSendAsync(new TException());

    /// <summary>
    /// Verifies the mocked <see cref="IHttpMessageHandler.SendAsync(HttpRequestMessage, CancellationToken)"/> has been called.
    /// </summary>
    /// <param name="mockHttpMessageHandler">The instance of the <see cref="Mock{IHttpMessageHandler}"/> to configure.</param>
    /// <param name="times">The instance of <see cref="Times"/> to verify the method has been called.</param>
    public static void VerifySendAsync(this Mock<IHttpMessageHandler> mockHttpMessageHandler, Times times)
        => mockHttpMessageHandler.Verify(m => m.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), times);
}
