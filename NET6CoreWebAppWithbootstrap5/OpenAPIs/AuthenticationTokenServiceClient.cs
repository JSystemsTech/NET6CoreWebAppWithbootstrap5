using Microsoft.Extensions.Options;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using System.Net.Http.Headers;

namespace AuthenticationTokenService
{
    public partial class AuthenticationTokenServiceClient
    {
        private AuthenticationTokenServiceAccessToken? _AccessToken { get; set; } 
        private bool _FetchingAccessToken { get; set; }

        [ActivatorUtilitiesConstructor] // This constructor will be used by DI
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            if (!_FetchingAccessToken) //prevent infinite loop while calling AuthenticateAsync
            {
                if (_AccessToken == null || _AccessToken.RefreshOnUtc <= DateTime.UtcNow)
                {
                    _FetchingAccessToken = true;
                    var apiResponse = AuthenticateAsync(ApplicationConfiguration.ApplicationSettings.AppId).GetAwaiter().GetResult();
                    if (!apiResponse.HasError)
                    {
                        _AccessToken = apiResponse.Value;
                    }
                    _FetchingAccessToken = false;
                }
                if (_AccessToken is AuthenticationTokenServiceAccessToken ac)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ac.Token);
                }
            }
        }
    }
}
