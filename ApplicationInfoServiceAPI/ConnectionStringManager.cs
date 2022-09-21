using ConnectionStringServiceAPI;

namespace ApplicationInfoServiceAPI
{
    public class ConnectionStringInfo
    {
        public string ConnectionString { get; private set; }
        public string ProviderName { get; private set; }
        public ConnectionStringInfo(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
        }
        internal void UpdateConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
    public interface IConnectionStringManager
    {
        ConnectionStringInfo? GetConnectionString(string name);
    }
    public class ConnectionStringManager : IConnectionStringManager
    {
        private ConnectionStringServiceAPIClient ConnectionStringServiceAPIClient { get; set; }
        private IDictionary<string, ConnectionStringInfo> ConnectionStrings { get; set; }
        public ConnectionStringManager(IConfiguration configuration, ConnectionStringServiceAPIClient connectionStringServiceAPIClient)
        {
            ConnectionStringServiceAPIClient = connectionStringServiceAPIClient;
            IConfigurationSection connectionStringsConfigurationSection = configuration.GetSection("ConnectionStrings");
            ConnectionStrings = connectionStringsConfigurationSection.GetChildren().ToDictionary(cs => cs.Key, cs => new ConnectionStringInfo(cs.GetSection("ProviderName").Value, cs.GetSection("ConnectionString").Value));

        }
        public ConnectionStringInfo? GetConnectionString(string name)
        {
            if (ConnectionStrings.TryGetValue(name, out ConnectionStringInfo? connectionString) && connectionString is ConnectionStringInfo)
            {
                if (string.IsNullOrWhiteSpace(connectionString.ConnectionString))
                {
                    ResolveConnectionString(name, connectionString);
                }
                return connectionString;
            }
            return null;
        }
        private void ResolveConnectionString(string name, ConnectionStringInfo connectionString)
        {
            var result = ConnectionStringServiceAPIClient.GetConnectionStringAsync(name).GetAwaiter().GetResult();
            if (!result.HasError)
            {
                connectionString.UpdateConnectionString(result.ConnectionString);
            }
        }
    }
}
