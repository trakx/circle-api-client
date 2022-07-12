using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using Trakx.Circle.ApiClient.Utils;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Circle.ApiClient;

public static partial class AddCircleClientExtension
{
    public static IServiceCollection AddCircleClient(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
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
        var options = Options.Create(apiConfiguration);
        services.AddSingleton(options);

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
