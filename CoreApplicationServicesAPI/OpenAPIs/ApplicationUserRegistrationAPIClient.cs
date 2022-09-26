using CoreApplicationServicesAPI;
using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.Models;
using System.Net.Http.Headers;

namespace ApplicationUserRegistrationAPI
{
    public partial class ApplicationUserRegistrationAPIClient
    {
        private APIAccessToken? _AccessToken { get; set; }
        private IDictionary<string, APIAccessToken> APIAccessTokens { get; set; }

        [ActivatorUtilitiesConstructor] // This constructor will be used by DI
        public ApplicationUserRegistrationAPIClient(HttpClient httpClient)
        : this("", httpClient) {
            APIAccessTokens = new Dictionary<string, APIAccessToken>();
        }
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            SetAccessToken(request);
        }
        private APIAccessToken? GetAccessToken(string appId)
        {
            if (APIAccessTokens.TryGetValue(appId, out APIAccessToken? accessToken) && accessToken != null)
            {
                return accessToken;
            }
            return null;
        }
        private async Task<bool> TryConfigureAccessTokenForAppAsync(string appId, ApplicationInfo appInfo)
        {
            var accessToken = GetAccessToken(appId);
            if (accessToken == null || accessToken.RefreshOnUtc <= DateTime.UtcNow)
            {
                var response = await AuthenticateAsync(appInfo);
                if (!response.HasError)
                {
                    return false;
                }
                accessToken = response.Value;
                if (APIAccessTokens.ContainsKey(appId))
                {
                    APIAccessTokens[appId] = accessToken;
                }
                else
                {
                    APIAccessTokens.Add(appId, accessToken);
                }
            }
            _AccessToken = accessToken;
            return true;
        }
        private async Task<APIAccessTokenAPIResponse> AuthenticateAsync(ApplicationInfo appInfo)
        {
            _baseUrl = appInfo.ApplicationAPIUrl;
            var response = await AuthenticateAsync(ApplicationConfiguration.ApplicationSettings.AppId);

            return response;
        }
        public async Task<BooleanAPIResponse> RegisterUserAsync(string appId, RegisterUserParameters parameters, ApplicationInfo appInfo)
        {
            if(await TryConfigureAccessTokenForAppAsync(appId, appInfo))
            {
                _baseUrl = appInfo.ApplicationAPIUrl;
                return  await RegisterUserAsync(parameters);
            }
            return new BooleanAPIResponse() { ErrorMessage= "Unable to configure access token for application API"};
        }
        public async Task<BooleanAPIResponse> UserIsRegisteredAsync(string appId, RegisterUserParameters parameters, ApplicationInfo appInfo)
        {
            if (await TryConfigureAccessTokenForAppAsync(appId, appInfo))
            {
                _baseUrl = appInfo.ApplicationAPIUrl;
                return await UserIsRegisteredAsync(parameters);
            }
            return new BooleanAPIResponse() { ErrorMessage = "Unable to configure access token for application API" };
        }
        private void SetAccessToken(HttpRequestMessage request)
        {
            if (_AccessToken is APIAccessToken ac)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ac.Token);
            }
        }
    }
}
