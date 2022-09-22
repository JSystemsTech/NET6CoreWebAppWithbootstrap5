namespace CoreApplicationServicesAPI.Models
{
    //Include namespace in Response class name so OpenAPI generated code has no conflictions with other OpenAPI refs
    public class APIResponse<T>
    {
        public T Value { get; private set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public string? ErrorMessage { get; private set; }
        private APIResponse(T value)
        {
            Value = value;
        }
        internal static APIResponse<T> Create(T value)
        => new APIResponse<T>(value)
        {
            Value = value
        };
        internal static APIResponse<T> Error(string errorMessage, T value)
        => new APIResponse<T>(value)
        {
            ErrorMessage = errorMessage,
            Value = value
        };
    }
}
