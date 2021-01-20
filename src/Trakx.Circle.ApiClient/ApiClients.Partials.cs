
using System.Net.Http;

namespace Trakx.Circle.ApiClient
{
    internal partial class AccountsClient
    {
#pragma warning disable S1172, IDE0060 // Unused method parameters should be removed
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
#pragma warning restore S1172, IDE0060 // Unused method parameters should be removed
        {
            CredentialProvider.AddCredentials(request);
        }
    }

}