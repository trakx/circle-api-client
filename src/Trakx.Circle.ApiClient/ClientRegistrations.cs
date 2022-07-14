using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Serilog;
using Trakx.Utils.Apis;


namespace Trakx.Circle.ApiClient;

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
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.AccountsClient"));
        
        services.AddHttpClient<IPaymentsClient, PaymentsClient>("Trakx.Circle.ApiClient.PaymentsClient")
            .AddPolicyHandler((s, request) =>
                Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<PaymentsClient>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.PaymentsClient"));
        services.AddHttpClient<ICardsClient, CardsClient>("Trakx.Circle.ApiClient.CardsClient")
            .AddPolicyHandler((s, request) =>
                Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<CardsClient>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.CardsClient"));
        
        services.AddHttpClient<IBankAccountsClient, BankAccountsClient>("Trakx.Circle.ApiClient.BankAccountsClient")
            .AddPolicyHandler((s, request) =>
                Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<BankAccountsClient>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.BankAccountsClient"));
        
        services.AddHttpClient<ISettlementsClient, SettlementsClient>("Trakx.Circle.ApiClient.SettlementsClient")
            .AddPolicyHandler((s, request) =>
                Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<SettlementsClient>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    
                    .WithPolicyKey("Trakx.Circle.ApiClient.SettlementsClient"));
        
        services.AddHttpClient<IChargebacksClient, ChargebacksClient>("Trakx.Circle.ApiClient.ChargebacksClient")
            .AddPolicyHandler((s, request) =>
                Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<ChargebacksClient>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.ChargebacksClient"));
        
        services.AddHttpClient<IReversalsClient, ReversalsClient>("Trakx.Circle.ApiClient.ReversalsClient")
            .AddPolicyHandler((s, request) =>
                Policy<HttpResponseMessage>
                    .Handle<ApiException>()
                    .Or<HttpRequestException>()
                    .OrTransientHttpStatusCode()
                    .WaitAndRetryAsync(delay,
                        onRetry: (result, timeSpan, retryCount, context) =>
                        {
                            var logger = Log.Logger.ForContext<ReversalsClient>();
                            logger.LogApiFailure(result, timeSpan, retryCount, context);
                        })
                    .WithPolicyKey("Trakx.Circle.ApiClient.ReversalsClient"));
        

    }
}
