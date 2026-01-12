using Trakx.Common.Attributes;

namespace Trakx.Circle.ApiClient;

public record CircleApiConfiguration
{
    /// <summary>
    /// The base URL of the Circle API.
    /// </summary>
    public Uri BaseUrl { get; init; } = new("about:blank");

    /// <summary>
    /// The API key to use to authenticate with the API.
    /// </summary>
    [AwsParameter, SecretEnvironmentVariable]
    public string ApiKey { get; init; } = null!;

    /// <summary>
    /// Initial delay used to wait after a failure before retrying.
    /// See <see cref="Polly.Contrib.WaitAndRetry.Backoff"/>.
    /// </summary>
    public int InitialRetryDelayInMilliseconds { get; init; } = 100;

    /// <summary>
    /// The maximum number of failed attempts to query the API
    /// before deciding that a request has failed.
    /// </summary>
    public int MaxRetryCount { get; init; } = 3;
}
