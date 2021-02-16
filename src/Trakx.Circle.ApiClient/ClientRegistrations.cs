using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;


namespace Trakx.Circle.ApiClient
{
    public static partial class AddCircleClientExtension
    {
        private static void AddClients(this IServiceCollection services, CircleApiConfiguration configuration)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromMilliseconds(configuration.InitialRetryDelayInMilliseconds ?? 100), 
                retryCount: configuration.MaxRetryCount ?? 3, fastFirst: true);
                                    
            services.AddHttpClient<IAccountsClient, AccountsClient>("Trakx.Circle.ApiClient.AccountsClient")
                .AddPolicyHandler((s, request) => 
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<AccountsClient>();
                            LogFailure(logger, result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.AccountsClient"));

        }
    }
}
