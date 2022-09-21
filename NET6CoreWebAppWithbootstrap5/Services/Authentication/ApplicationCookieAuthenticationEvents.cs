using AuthenticationTokenService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text;
using CoreApplicationServicesAPI;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using Aspose.Cells;
using Aspose.Finance.Ofx;
using System.Xml.Linq;

namespace NET6CoreWebAppWithbootstrap5.Services.Authentication
{
    public class ApplicationCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly ApplicationPricipalIdentityManager ApplicationPricipalIdentityManager;

        public ApplicationCookieAuthenticationEvents(ApplicationPricipalIdentityManager applicationPricipalIdentityManager)
        {
            ApplicationPricipalIdentityManager = applicationPricipalIdentityManager;
        }
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if(context.Principal is ClaimsPrincipal user && 
                await ApplicationPricipalIdentityManager.IsValidPrincipalAsync(user)
            )
            {
                await Task.CompletedTask;
            }
            else {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            
        }
    }
    public class RefreshClaimsTransformation : IClaimsTransformation
    {
        private readonly ApplicationPricipalIdentityManager ApplicationPricipalIdentityManager;

        public RefreshClaimsTransformation(ApplicationPricipalIdentityManager applicationPricipalIdentityManager)
        {
            ApplicationPricipalIdentityManager = applicationPricipalIdentityManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            ClaimsPrincipal user = principal.Clone();
            if (user.Identity is ClaimsIdentity identity &&
                await ApplicationPricipalIdentityManager.IsValidPrincipalAsync(principal) &&
                (await ApplicationPricipalIdentityManager.CreateAuthenticatedUserAsync(identity)).user is ClaimsPrincipal newUser

            )
            {
                return newUser;
            }
            await Task.CompletedTask;
            return principal;
        }
    }
    public class AppUserData
    {
        public string Name { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleInitial { get; set; } = "";
        public string EDIPI { get; set; } = "";
        public string Email { get; set; } = "";
        public string? LastLogin { get; set; }
    }
    public static class TokenClaimExtensions
    {
       // string 
    }
    public class ApplicationPricipalIdentityManager
    {
        private readonly AuthenticationTokenServiceClient AuthTokenServiceClient;

        public ApplicationPricipalIdentityManager(AuthenticationTokenServiceClient authTokenServiceClient)
        {
            AuthTokenServiceClient = authTokenServiceClient;
        }
        public AppUserData? GetAppUserData(HttpContext httpContext)
        {
            if (httpContext.User.Identity is IIdentity identity &&
                identity.IsAuthenticated &&
                httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData) is Claim tokenClaim &&
                AuthTokenServiceClient.ParseTokenAsync(tokenClaim.Value).GetAwaiter().GetResult() is AuthTokenResponseAuthenticationTokenServiceAPIResponse response
                && !response.HasError
            )
            {
                return new AppUserData { 
                    Name = response.Value.Claims.FirstOrDefault(c=> c.Name == "Name") is TokenClaim tc ? tc.Value: "",
                    FirstName = response.Value.Claims.FirstOrDefault(c => c.Name == "FirstName") is TokenClaim tc1 ? tc1.Value : "",
                    LastName = response.Value.Claims.FirstOrDefault(c => c.Name == "LastName") is TokenClaim tc2 ? tc2.Value : "",
                    MiddleInitial = response.Value.Claims.FirstOrDefault(c => c.Name == "MiddleInitial") is TokenClaim tc3 ? tc3.Value : "",
                    EDIPI = response.Value.Claims.FirstOrDefault(c => c.Name == "EDIPI") is TokenClaim tc4 ? tc4.Value : "",
                    Email = response.Value.Claims.FirstOrDefault(c => c.Name == "Email") is TokenClaim tc5 ? tc5.Value : ""
                };
            }
            return null;
        }
        public async Task<bool> IsValidPrincipalAsync(ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData) is Claim tokenClaim &&
                (await AuthTokenServiceClient.ParseTokenAsync(tokenClaim.Value)) is AuthTokenResponseAuthenticationTokenServiceAPIResponse response
                && !response.HasError
                && response.Value.IsValid
            )
            {
                return true;
            }
            return false;
        }
        private async Task<IEnumerable<Claim>> GetIdentityClaimsAsync(AuthTokenResponse authTokenResponse)
        {
            List<Claim> claims = new List<Claim>();
            if (authTokenResponse.Claims.FirstOrDefault(m => m.Name == "Roles") is TokenClaim tokenClaim)
            {
                foreach (string role in tokenClaim.Values)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            claims.Add(new Claim(ClaimTypes.UserData, authTokenResponse.Value));
            await Task.CompletedTask;
            return claims;
        }
        
        public async Task<(ClaimsPrincipal? user, AuthTokenResponse? authToken)> CreateAuthenticatedUserAsync(ClaimsIdentity newIdentity)
        => newIdentity.FindFirst(ClaimTypes.UserData) is Claim claim ? await CreateAuthenticatedUserAsync(claim.Value) : (null, null);
        public async Task<(ClaimsPrincipal? user, AuthTokenResponse? authToken)> CreateAuthenticatedUserAsync(string authTokenStr)
        {
            // Clone current identity
            if ((await AuthTokenServiceClient.ParseTokenAsync(authTokenStr)) is AuthTokenResponseAuthenticationTokenServiceAPIResponse response && !response.HasError)
            {
                return await CreateAuthenticatedUserAsync(response.Value);
            }
            await Task.CompletedTask;
            return (null,null);
        }
        public async Task<ClaimsIdentity> CreateClaimsIdentityAsync(AuthTokenResponse authToken, string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            var applicationClaimsIdentity = new ClaimsIdentity(await GetIdentityClaimsAsync(authToken), authenticationScheme);
            await Task.CompletedTask;
            return applicationClaimsIdentity;
        }
        public async Task<(ClaimsPrincipal user, AuthTokenResponse authToken)> CreateAuthenticatedUserAsync(AuthTokenResponse authToken, string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            return (new ClaimsPrincipal(await CreateClaimsIdentityAsync(authToken, authenticationScheme)), authToken);
        }
        public async Task<AuthTokenResponse> CreateTokenAsync( string edipi, string name, string firstName, string lastName, string middleInitial, string email)
        {
            var response = await AuthTokenServiceClient.CreateTokenAsync(new TokenClaim[] {
                new TokenClaim(){ Name = "EDIPI", Value = edipi },
                new TokenClaim(){ Name = "Name", Value = name },
                new TokenClaim(){ Name = "FirstName", Value = firstName },
                new TokenClaim(){ Name = "LastName", Value = lastName },
                new TokenClaim(){ Name = "MiddleInitial", Value = middleInitial },
                new TokenClaim(){ Name = "Email", Value = email }
            });
            return response.Value;
        }
        private async Task SignInAsync(HttpContext httpContext, AuthTokenResponse authToken)
        {
            var userInfo = await CreateAuthenticatedUserAsync(authToken);
            if (authToken != null && userInfo.user is ClaimsPrincipal user)
            {
                var applicationAuthProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = authToken.ExpirationDate,
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    IssuedUtc = DateTime.UtcNow,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, applicationAuthProperties);
            }
        }
        public async Task SignInAsync(HttpContext httpContext, string edipi, string name, string firstName, string lastName, string middleInitial, string email)
        {
            await SignInAsync(httpContext, await CreateTokenAsync(edipi, name, firstName, lastName, middleInitial, email));
        }
    }
    public static class SSOAuthSchemeConstants
    { 
        public const string SSOAuthScheme = "SSOAuthScheme";
        public static AuthenticationBuilder AddSSO(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<SSOAuthSchemeOptions, SSOAuthHandler>(SSOAuthScheme, options => { });
        }
    }
    public class SSOAuthSchemeOptions
        : AuthenticationSchemeOptions
    { }
    public class SSOAuthHandler
        : AuthenticationHandler<SSOAuthSchemeOptions>
    {
        private readonly AuthenticationTokenServiceClient AuthTokenServiceClient;
        private readonly CoreApplicationServicesAPIClient CoreApplicationServicesAPIClient;
        private readonly ApplicationPricipalIdentityManager ApplicationPricipalIdentityManager;
        
        public SSOAuthHandler(
            IOptionsMonitor<SSOAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AuthenticationTokenServiceClient authTokenServiceClient,
            CoreApplicationServicesAPIClient coreApplicationServicesAPIClient,
            ApplicationPricipalIdentityManager applicationPricipalIdentityManager
            )
            : base(options, logger, encoder, clock)
        {
            AuthTokenServiceClient = authTokenServiceClient;
            CoreApplicationServicesAPIClient = coreApplicationServicesAPIClient;
            ApplicationPricipalIdentityManager = applicationPricipalIdentityManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var response = await CoreApplicationServicesAPIClient.GetApplicationInfoAsync(ApplicationConfiguration.ApplicationSettings.AppId);
            if (!response.HasError)
            {
                var webAppInfo = response.Value;
                // validation comes in here
                if (!Request.Headers.ContainsKey(webAppInfo.SsO_Header))
                {
                    return AuthenticateResult.Fail("Header Not Found.");
                }

                string header = Request.Headers[webAppInfo.SsO_Header].ToString();
                if((await AuthTokenServiceClient.ParseSSOTokenAsync(header)) is SSOTokenDataResponseAuthenticationTokenServiceAPIResponse ssoResponse && !ssoResponse.HasError)
                {
                    var data = ssoResponse.Value;
                    var authToken = await ApplicationPricipalIdentityManager.CreateTokenAsync(data.Edipi, $"{data.FirstName} {data.LastName}", data.FirstName, data.LastName, data.MiddleInitial, data.Email);
                    var userInfo = await ApplicationPricipalIdentityManager.CreateAuthenticatedUserAsync(authToken, Scheme.Name);
                    // generate AuthenticationTicket from the Identity
                    // and current authentication scheme
                    var ticket = new AuthenticationTicket(userInfo.user, Scheme.Name);

                    // pass on the ticket to the middleware
                    return AuthenticateResult.Success(ticket);
                }
                return AuthenticateResult.Fail("Unable to parse SSO Token");
            }
            return AuthenticateResult.Fail("Unable to locate application info");
        }
    }

}
