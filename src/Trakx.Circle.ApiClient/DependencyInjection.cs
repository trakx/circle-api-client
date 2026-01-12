using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Trakx.Common.ApiClient;
using Trakx.Common.Configuration;

namespace Trakx.Circle.ApiClient;

public static class DependencyInjection
{
    public static IServiceCollection AddCircleClient(this IServiceCollection services, IConfiguration configuration)
    {
        var apiConfiguration = configuration.GetConfiguration<CircleApiConfiguration>();
        return services.AddCircleClient(apiConfiguration);
    }

    public static IServiceCollection AddCircleClient(this IServiceCollection services, CircleApiConfiguration apiConfiguration)
        => services.AddCommonDependencies(apiConfiguration);

    private static IServiceCollection AddCommonDependencies(this IServiceCollection services, CircleApiConfiguration apiConfiguration)
    {
        services.AddSingleton(Options.Create(apiConfiguration));
        services.AddSingleton(apiConfiguration);
        services.AddSingleton<ClientConfigurator>();
        services.AddSingleton<ICircleCredentialsProvider, ApiKeyCredentialsProvider>();

        return services.AddApiClientsOfBaseTypeWithHttpClient<ICircleApiClientBase>(
            new ApiClientWithHttpClientRetryOptions
            {
                RetryCount = apiConfiguration.MaxRetryCount,
                MedianFirstRetryDelayMillis = apiConfiguration.InitialRetryDelayInMilliseconds
            },
            baseUrl: apiConfiguration.BaseUrl);
    }
}
