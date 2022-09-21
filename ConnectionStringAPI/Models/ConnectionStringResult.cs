namespace ConnectionStringAPI.Models
{
    public class ConnectionStringResult : APIResponse
    {
        public string ConnectionString { get; private set; }
        public ConnectionStringResult(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public static ConnectionStringResult Error(string errorMessage)
        => new ConnectionStringResult("")
        {
            ErrorMessage = errorMessage
        };
    }
}
