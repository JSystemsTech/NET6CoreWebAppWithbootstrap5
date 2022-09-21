using ConnectionStringAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConnectionStringAPI
{
    //manages connected Users
    public class UserManager
    {
        private static readonly IDictionary<string, string> _APIAllowedUsers = new Dictionary<string, string>() {
            {"Some_Generated_Key_For_MyTestApp","MyTestApp_AppUser" },
            {"AuthenticationTokenService", "AuthenticationTokenService_AppUser" }
        };
        public UserManager() { }
        public bool IsAllowedUser(string appId)
        {
            return _APIAllowedUsers.ContainsKey(appId);
        }
        public AccessToken GetAccessToken(string appId)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.JwtSettings.EncryptionKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: Configuration.JwtSettings.Issuer,
                audience: Configuration.JwtSettings.Audience,
                claims: new List<Claim>() { new Claim(ClaimTypes.System, appId) },
                expires: DateTime.Now.AddMinutes(Configuration.JwtSettings.ValidForCalculated),
                signingCredentials: signinCredentials
            );
            DateTime expiresUtc = DateTime.UtcNow.AddMinutes(Configuration.JwtSettings.ValidForCalculated);
            DateTime refreshOnUtc = DateTime.UtcNow.AddMinutes(Configuration.JwtSettings.RefreshAfter);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new AccessToken(expiresUtc, refreshOnUtc, token);
        }
    }
}
