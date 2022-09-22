using Newtonsoft.Json.Linq;

namespace CoreApplicationServicesAPI.Models
{
    
    //Include namespace in Access Token class name so OpenAPI generated code has no conflictions with other OpenAPI refs
    public class APIAccessToken
    {
        public DateTime ExpiresUtc { get; private set; }
        public DateTime RefreshOnUtc { get; private set; }
        public string Token { get; private set; }
        public APIAccessToken(DateTime expiresUtc, DateTime refreshOnUtc, string token)
        {
            ExpiresUtc = expiresUtc;
            RefreshOnUtc = refreshOnUtc;
            Token = token;
        }
        public static APIAccessToken Default = new APIAccessToken(DateTime.UtcNow, DateTime.UtcNow, "");
    }
}
