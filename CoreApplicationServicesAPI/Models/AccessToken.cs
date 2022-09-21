using Newtonsoft.Json.Linq;

namespace CoreApplicationServicesAPI.Models
{
    //Include namespace in Response class name so OpenAPI generated code has no conflictions with other OpenAPI refs
    public class CoreApplicationServicesAPIResponse<T>
    {
        public T Value { get; private set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public string? ErrorMessage { get; private set; }
        private CoreApplicationServicesAPIResponse(T value)
        {
            Value = value;
        }
        internal static CoreApplicationServicesAPIResponse<T> Create(T value)
        => new CoreApplicationServicesAPIResponse<T>(value)
        {
            Value = value
        };
        internal static CoreApplicationServicesAPIResponse<T> Error(string errorMessage, T value)
        => new CoreApplicationServicesAPIResponse<T>(value)
        {
            ErrorMessage = errorMessage,
            Value = value
        };
    }
    //Include namespace in Access Token class name so OpenAPI generated code has no conflictions with other OpenAPI refs
    public class CoreApplicationServicesAPIAccessToken
    {
        public DateTime ExpiresUtc { get; private set; }
        public DateTime RefreshOnUtc { get; private set; }
        public string Token { get; private set; }
        public CoreApplicationServicesAPIAccessToken(DateTime expiresUtc, DateTime refreshOnUtc, string token)
        {
            ExpiresUtc = expiresUtc;
            RefreshOnUtc = refreshOnUtc;
            Token = token;
        }
        public static CoreApplicationServicesAPIAccessToken Default = new CoreApplicationServicesAPIAccessToken(DateTime.UtcNow, DateTime.UtcNow, "");
    }
}
