using Trakx.Common.ApiClient;

namespace Trakx.Circle.ApiClient;

internal abstract class AuthorisedClient
{
    protected readonly ICredentialsProvider CredentialProvider;

    protected AuthorisedClient(ClientConfigurator configurator)
    {
        CredentialProvider = configurator.CredentialsProvider;
    }
}
