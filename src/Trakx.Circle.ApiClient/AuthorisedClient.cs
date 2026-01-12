using Trakx.Common.ApiClient;

namespace Trakx.Circle.ApiClient;

internal abstract class AuthorisedClient
{
    private readonly ICredentialsProvider _credentialProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _httpClientName;

    protected AuthorisedClient(ClientConfigurator configurator)
    {
        _credentialProvider = configurator.CredentialsProvider;
        _httpClientFactory = configurator.HttpClientFactory;
        _httpClientName = GetType().FullName!;
    }

    protected async Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var httpClient = _httpClientFactory.CreateClient(_httpClientName);
        return httpClient;
    }

    protected async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        HttpRequestMessage httpRequestMessage = new();
        await _credentialProvider.AddCredentialsAsync(httpRequestMessage);
        return httpRequestMessage;
    }
}