using Microsoft.Extensions.Logging;
using Trakx.Common.ApiClient;
using Trakx.Common.Logging;

namespace Trakx.Circle.ApiClient;

public interface ICircleCredentialsProvider : ICredentialsProvider { }

public sealed class ApiKeyCredentialsProvider(CircleApiConfiguration configuration) : ICircleCredentialsProvider
{
    private static readonly ILogger Logger = LoggerProvider.Create<ApiKeyCredentialsProvider>();

    public void AddCredentials(HttpRequestMessage msg)
    {
        msg.Headers.Add("Authorization", $"Bearer {configuration.ApiKey}");
        Logger.LogTrace("Circle API Key added to request headers.");
    }

    public async Task AddCredentialsAsync(HttpRequestMessage msg)
    {
        await Task.CompletedTask;
        AddCredentials(msg);
    }
}
