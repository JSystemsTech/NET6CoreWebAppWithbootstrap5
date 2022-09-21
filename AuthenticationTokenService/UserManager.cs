using AuthenticationTokenService.Models;
using CoreApplicationServicesAPI;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationTokenService
{
    //manages connected Users
    public class UserManager
    {
        CoreApplicationServicesAPIClient CoreApplicationServicesAPIClient { get; set; }
        public UserManager(CoreApplicationServicesAPIClient coreApplicationServicesAPIClient) {
            CoreApplicationServicesAPIClient = coreApplicationServicesAPIClient;

        }
        public async Task<bool> IsAllowedUserAsync(string appId)
        => (await CoreApplicationServicesAPIClient.ApplicationExistsAsync(appId)).Value;
        public AuthenticationTokenServiceAccessToken GetAccessToken(string appId)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConfiguration.JwtSettings.EncryptionKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: ApplicationConfiguration.JwtSettings.Issuer,
                audience: ApplicationConfiguration.JwtSettings.Audience,
                claims: new List<Claim>() { new Claim(ClaimTypes.System, appId) },
                expires: DateTime.Now.AddMinutes(ApplicationConfiguration.JwtSettings.ValidForCalculated),
                signingCredentials: signinCredentials
            );
            DateTime expiresUtc = DateTime.UtcNow.AddMinutes(ApplicationConfiguration.JwtSettings.ValidForCalculated);
            DateTime refreshOnUtc = DateTime.UtcNow.AddMinutes(ApplicationConfiguration.JwtSettings.RefreshAfter);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new AuthenticationTokenServiceAccessToken(expiresUtc, refreshOnUtc, token);
        }
    }
}
