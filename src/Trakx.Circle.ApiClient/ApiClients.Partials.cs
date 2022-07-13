
namespace Trakx.Circle.ApiClient
{
    internal partial class AccountsClient
    {
#pragma warning disable S1172
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
#pragma warning restore S1172
        {
            CredentialProvider.AddCredentials(request);
        }
    }
    internal partial class PaymentsClient
    {
#pragma warning disable S1172
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
#pragma warning restore S1172
        {
            CredentialProvider.AddCredentials(request);
        }
    }

}