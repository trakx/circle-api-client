using System.ComponentModel.DataAnnotations;
using Trakx.Utils.Attributes;

namespace Trakx.Circle.ApiClient
{
    public record CircleApiConfiguration
    {
#nullable disable

        public string BaseUrl { get; init; }

        [AwsParameter, SecretEnvironmentVariable]
        public string ApiKey { get; init; }

        [AwsParameter, SecretEnvironmentVariable]
        public string ApiSecret { get; set; }

        public double? InitialRetryDelayInMilliseconds { get; init; }
        public int? MaxRetryCount { get; init; }
#nullable restore
    }
}
