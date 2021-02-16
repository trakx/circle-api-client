using Trakx.Utils.Attributes;

namespace Trakx.Circle.ApiClient
{
    public record CircleApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; init; }
        [SecretEnvironmentVariable]
        public string ApiKey { get; init; }
        public double? InitialRetryDelayInMilliseconds { get; init; }
        public int? MaxRetryCount { get; init; }
#nullable restore
    }
}