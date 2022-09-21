using Newtonsoft.Json.Linq;

namespace AuthenticationTokenService.Models
{
    //Include namespace in Response class name so OpenAPI generated code has no conflictions with other OpenAPI refs
    public class AuthenticationTokenServiceAPIResponse<T>
    {
        public T Value { get; private set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public string? ErrorMessage { get; private set; }
        private AuthenticationTokenServiceAPIResponse(T value)
        {
            Value = value;
        }
        internal static AuthenticationTokenServiceAPIResponse<T> Create(T value)
        => new AuthenticationTokenServiceAPIResponse<T>(value)
        {
            Value = value
        };
        internal static AuthenticationTokenServiceAPIResponse<T> Error(string errorMessage, T value)
        => new AuthenticationTokenServiceAPIResponse<T>(value)
        {
            ErrorMessage = errorMessage,
            Value = value
        };
    }
    public class AuthenticationTokenServiceAccessToken
    {
        public DateTime ExpiresUtc { get; private set; }
        public DateTime RefreshOnUtc { get; private set; }
        public string Token { get; private set; }
        public AuthenticationTokenServiceAccessToken(DateTime expiresUtc, DateTime refreshOnUtc, string token)
        {
            ExpiresUtc = expiresUtc;
            RefreshOnUtc = refreshOnUtc;
            Token = token;
        }
        public static AuthenticationTokenServiceAccessToken Default = new AuthenticationTokenServiceAccessToken(DateTime.UtcNow, DateTime.UtcNow, "");

    }
    public class SSOTokenDataResponse 
    {
        public string EDIPI { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string MiddleInitial { get; private set; }
        public SSOTokenDataResponse(string edipi, string email, string firstName, string lastName, string middleInitial)
        {
            EDIPI = edipi;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            MiddleInitial = middleInitial;
        }
        public static SSOTokenDataResponse Default = new SSOTokenDataResponse("","","","","");
    }
    public class SSOTokenParameters
    {
        public string EDIPI { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleInitial { get; set; } = "";
    }
}
