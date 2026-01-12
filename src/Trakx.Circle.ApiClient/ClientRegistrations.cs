using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace Trakx.Circle.ApiClient
{
    public static partial class AddCircleClientExtension
    {
        private static void AddClients(this IServiceCollection services, CircleApiConfiguration configuration)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromMilliseconds(configuration.InitialRetryDelayInMilliseconds ?? 100),
                retryCount: configuration.MaxRetryCount ?? 10, fastFirst: true);
            
            services.AddHttpClient<IAccountsClient, AccountsClient>("Trakx.Circle.ApiClient.AccountsClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<AccountsClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.AccountsClient"));

        
            services.AddHttpClient<IPaymentsClient, PaymentsClient>("Trakx.Circle.ApiClient.PaymentsClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<PaymentsClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.PaymentsClient"));

        
            services.AddHttpClient<ICardsClient, CardsClient>("Trakx.Circle.ApiClient.CardsClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<CardsClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.CardsClient"));

        
            services.AddHttpClient<IBankAccountsClient, BankAccountsClient>("Trakx.Circle.ApiClient.BankAccountsClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<BankAccountsClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.BankAccountsClient"));

        
            services.AddHttpClient<ISettlementsClient, SettlementsClient>("Trakx.Circle.ApiClient.SettlementsClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<SettlementsClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.SettlementsClient"));

        
            services.AddHttpClient<IChargebacksClient, ChargebacksClient>("Trakx.Circle.ApiClient.ChargebacksClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<ChargebacksClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.ChargebacksClient"));

        
            services.AddHttpClient<IReversalsClient, ReversalsClient>("Trakx.Circle.ApiClient.ReversalsClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<ReversalsClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.ReversalsClient"));

        
            services.AddHttpClient<IBusinessAccountClient, BusinessAccountClient>("Trakx.Circle.ApiClient.BusinessAccountClient", client => client.BaseAddress = configuration.BaseUrl)
                .AddPolicyHandler((s, request) =>
                    Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = s.GetRequiredService<ILogger<BusinessAccountClient>>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.BusinessAccountClient"));

        }
    }
}
