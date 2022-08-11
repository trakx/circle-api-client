using Microsoft.Extensions.Options;
using Trakx.Circle.ApiClient.Utils;

namespace Trakx.Circle.ApiClient;

internal class ClientConfigurator
{
    public ICircleCredentialsProvider CredentialsProvider { get; }
    public CircleApiConfiguration ApiConfiguration { get; }

    public ClientConfigurator(CircleApiConfiguration configuration, ICircleCredentialsProvider credentialsProvider)
    {
        CredentialsProvider = credentialsProvider;
        ApiConfiguration = configuration;
    }
}
