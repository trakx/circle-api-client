using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using Trakx.Circle.ApiClient.Utils;
using Trakx.Utils.DateTimeHelpers;

namespace Trakx.Circle.ApiClient
{
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

        private static void LogFailure(ILogger logger, DelegateResult<HttpResponseMessage> result, TimeSpan timeSpan, int retryCount, Context context)
        {
            if (result.Exception != null)
            {
                logger.Warning(result.Exception, "An exception occurred on retry {RetryAttempt} for {PolicyKey}. Retrying in {SleepDuration}ms.",
                    retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            }
            else
            {
                logger.Warning("A non success code {StatusCode} with reason {Reason} and content {Content} was received on retry {RetryAttempt} for {PolicyKey}. Retrying in {SleepDuration}ms.",
                    (int)result.Result.StatusCode, result.Result.ReasonPhrase,
                    result.Result.Content?.ReadAsStringAsync().GetAwaiter().GetResult(),
                    retryCount, context.PolicyKey, timeSpan.TotalMilliseconds);
            }
        }
    }
}