using Trakx.Common.ApiClient;

namespace Trakx.Circle.ApiClient;

internal abstract class AuthorisedClient(ClientConfigurator configurator)
{
    private readonly ICredentialsProvider _credentialProvider = configurator.CredentialsProvider;

    protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        HttpRequestMessage httpRequestMessage = new();
        await _credentialProvider.AddCredentialsAsync(httpRequestMessage);
        return httpRequestMessage;
    }
}