using Trakx.Utils.Attributes;
using Trakx.Utils.Testing;

namespace Trakx.Circle.ApiClient.Tests
{
    public record Secrets : SecretsBase
    {
        [SecretEnvironmentVariable(nameof(CircleApiConfiguration), nameof(CircleApiConfiguration.ApiKey))]
        public string? CircleApiKey { get; init; }
    }
    
}