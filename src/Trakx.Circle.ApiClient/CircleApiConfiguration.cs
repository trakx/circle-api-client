using System.Collections.Generic;

namespace Trakx.Circle.ApiClient
{
    public record CircleApiConfiguration
    {
#nullable disable
        public string BaseUrl { get; init; }
        public string ApiKey { get; init; }
        public string ApiSecret { get; init; }
        public List<string> FavouriteExchanges { get; init; }
#nullable restore
    }
}