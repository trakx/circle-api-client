using Trakx.Utils.Apis;

namespace Trakx.Circle.ApiClient
{
    internal abstract class AuthorisedClient
    {
        protected readonly ICredentialsProvider CredentialProvider;
        protected string BaseUrl { get; }

        protected AuthorisedClient(ClientConfigurator configurator)
        {
            CredentialProvider = configurator.CredentialsProvider;
            BaseUrl = configurator.ApiConfiguration.BaseUrl;
        }
    }
}