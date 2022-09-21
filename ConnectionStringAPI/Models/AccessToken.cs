namespace ConnectionStringAPI.Models
{
    public abstract class APIResponse
    {
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public string? ErrorMessage { get; protected set; }
    }
    public class AccessToken : APIResponse
    {
        public DateTime ExpiresUtc { get; private set; }
        public DateTime RefreshOnUtc { get; private set; }
        public string Token { get; private set; }
        public AccessToken(DateTime expiresUtc, DateTime refreshOnUtc, string token)
        {
            ExpiresUtc = expiresUtc;
            RefreshOnUtc = refreshOnUtc;
            Token = token;
        }
        public static AccessToken Error(string errorMessage)
        => new AccessToken(DateTime.UtcNow, DateTime.UtcNow, "")
        {
            ErrorMessage = errorMessage
        };
    }
}
