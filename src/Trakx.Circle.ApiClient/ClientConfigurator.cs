namespace Trakx.Circle.ApiClient;

internal class ClientConfigurator
{
    public ICircleCredentialsProvider CredentialsProvider { get; }
    public CircleApiConfiguration ApiConfiguration { get; }
    public IHttpClientFactory HttpClientFactory { get; }

    public ClientConfigurator(
        CircleApiConfiguration configuration,
        ICircleCredentialsProvider credentialsProvider,
        IHttpClientFactory httpClientFactory)
    {
        CredentialsProvider = credentialsProvider;
        ApiConfiguration = configuration;
        HttpClientFactory = httpClientFactory;
    }
}
