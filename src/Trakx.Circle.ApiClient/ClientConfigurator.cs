using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Utils.Apis;

namespace Trakx.Circle.ApiClient
{
    internal class ClientConfigurator
    {
        private readonly IServiceProvider _serviceProvider;

        public ClientConfigurator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ApiConfiguration = serviceProvider.GetService<IOptions<CircleApiConfiguration>>()!.Value;
        }

        public CircleApiConfiguration ApiConfiguration { get; }

        public ICredentialsProvider GetCredentialProvider(Type clientType)
        {
            switch (clientType.Name)
            {
                case nameof(MarketDataClient):
                //case nameof(ExchangesClient):
                    return new NoCredentialsProvider();
                default:
                    return _serviceProvider.GetService<ICredentialsProvider>()!;
            }
        }
    }
}