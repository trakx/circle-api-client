using Trakx.Common.Attributes;

namespace Trakx.Circle.ApiClient;

public record CircleApiConfiguration
{
    public Uri BaseUrl { get; init; } = new("about:blank");

    [AwsParameter, SecretEnvironmentVariable]
    public string ApiKey { get; init; } = null!;
    public double? InitialRetryDelayInMilliseconds { get; init; }
    public int? MaxRetryCount { get; init; }
}
