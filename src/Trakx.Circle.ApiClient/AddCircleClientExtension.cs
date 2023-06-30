using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trakx.Circle.ApiClient.Utils;
using Trakx.Common.DateAndTime;

namespace Trakx.Circle.ApiClient;

public static partial class AddCircleClientExtension
{
    public static IServiceCollection AddCircleClient(
        this IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = configuration.GetSection(nameof(CircleApiConfiguration))
            .Get<CircleApiConfiguration>()!;
        services.Configure<CircleApiConfiguration>(
            configuration.GetSection(nameof(CircleApiConfiguration)));
        AddCommonDependencies(services, apiConfig);

        return services;
    }

    public static IServiceCollection AddCircleClient(
        this IServiceCollection services, CircleApiConfiguration apiConfiguration)
    {
        services.AddSingleton(apiConfiguration);

        AddCommonDependencies(services, apiConfiguration);

        return services;
    }

    private static void AddCommonDependencies(IServiceCollection services, CircleApiConfiguration apiConfiguration)
    {
        services.AddSingleton<ClientConfigurator>();
        services.AddSingleton<ICircleCredentialsProvider, ApiKeyCredentialsProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        AddClients(services, apiConfiguration);
    }
}
