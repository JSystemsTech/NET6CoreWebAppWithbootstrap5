using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace ConnectionStringServiceAPI
{
    public class ConnectionStringServiceAPIClientOptions
    {
        public static string ConfigurationSection => "ConnectionStringServiceAPIClientOptions";
        public string Url { get; set; } = "";
        public string AppId { get; set; } = "";
    }
    public partial class ConnectionStringServiceAPIClient
    {
        private AccessToken? _AccessToken { get; set; }
        private bool _FetchingAccessToken { get; set; }
        private string _AppId { get; set; }

        [ActivatorUtilitiesConstructor] // This constructor will be used by DI
        public ConnectionStringServiceAPIClient(HttpClient httpClient, IOptions<ConnectionStringServiceAPIClientOptions> clientOptions)
        : this(clientOptions.Value.Url, httpClient) // Call generated ctor
        {
            _AppId = clientOptions.Value.AppId;
        }
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            SetAccessToken(request);
        }
        private void SetAccessToken(HttpRequestMessage request)
        {
            if (!_FetchingAccessToken) //prevent infinite loop while calling AuthenticateAsync
            {
                if (_AccessToken == null || _AccessToken.RefreshOnUtc <= DateTime.UtcNow)
                {
                    _FetchingAccessToken = true;
                    _AccessToken = AuthenticateAsync(_AppId).GetAwaiter().GetResult();
                    _FetchingAccessToken = false;
                }
                if (_AccessToken is AccessToken ac)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ac.Token);
                }
            }
        }
    }
}
