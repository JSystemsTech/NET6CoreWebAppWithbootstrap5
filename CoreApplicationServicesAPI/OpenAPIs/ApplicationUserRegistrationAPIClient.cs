using CoreApplicationServicesAPI;
using CoreApplicationServicesAPI.Models;
using System.Net.Http.Headers;

namespace ApplicationUserRegistrationAPI
{
    public partial class ApplicationUserRegistrationAPIClient
    {
        private ApplicationUserRegistrationAPIAccessToken? _AccessToken { get; set; }
        private IDictionary<string, ApplicationUserRegistrationAPIAccessToken> APIAccessTokens { get; set; }

        [ActivatorUtilitiesConstructor] // This constructor will be used by DI
        public ApplicationUserRegistrationAPIClient(HttpClient httpClient)
        : this("", httpClient) {
            APIAccessTokens = new Dictionary<string, ApplicationUserRegistrationAPIAccessToken>();
        }
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            SetAccessToken(request);
        }
        private ApplicationUserRegistrationAPIAccessToken? GetAccessToken(string appId)
        {
            if (APIAccessTokens.TryGetValue(appId, out ApplicationUserRegistrationAPIAccessToken? accessToken) && accessToken != null)
            {
                return accessToken;
            }
            return null;
        }
        private async Task<bool> TryConfigureAccessTokenForAppAsync(string appId, ApplicationAPIConfig config)
        {
            var accessToken = GetAccessToken(appId);
            if (accessToken == null || accessToken.RefreshOnUtc <= DateTime.UtcNow)
            {
                var response = await AuthenticateAsync(config);
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
        private async Task<ApplicationUserRegistrationAPIAccessTokenApplicationUserRegistrationAPIResponse> AuthenticateAsync(ApplicationAPIConfig config)
        {
            _baseUrl = config.BaseUrl;
            var response = await AuthenticateAsync(ApplicationConfiguration.ApplicationSettings.AppId);

            return response;
        }
        public async Task<BooleanApplicationUserRegistrationAPIResponse> RegisterUserAsync(string appId, RegisterUserParameters parameters, ApplicationAPIConfig config)
        {
            if(await TryConfigureAccessTokenForAppAsync(appId, config))
            {
                _baseUrl = config.BaseUrl;
                return  await RegisterUserAsync(parameters);
            }
            return new BooleanApplicationUserRegistrationAPIResponse() { ErrorMessage= "Unable to configure access token for application API"};
        }
        public async Task<BooleanApplicationUserRegistrationAPIResponse> UserIsRegisteredAsync(string appId, RegisterUserParameters parameters, ApplicationAPIConfig config)
        {
            if (await TryConfigureAccessTokenForAppAsync(appId, config))
            {
                _baseUrl = config.BaseUrl;
                return await UserIsRegisteredAsync(parameters);
            }
            return new BooleanApplicationUserRegistrationAPIResponse() { ErrorMessage = "Unable to configure access token for application API" };
        }
        private void SetAccessToken(HttpRequestMessage request)
        {
            if (_AccessToken is ApplicationUserRegistrationAPIAccessToken ac)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ac.Token);
            }
        }
    }
}
