namespace ConnectionStringAPI
{
    public class ConnectionStringManager
    {
        private static readonly IDictionary<string, string> _ConnectionStringMap = new Dictionary<string, string>() {
            {"AdventureWorksDb","Data Source=JMAC_LG_PC\\SQLEXPRESS;Initial Catalog=AdventureWorks2019;Persist Security Info=True;User ID=WebAppUser;Password=K$j254a2778899;Trusted_Connection=True" }
        };
        public ConnectionStringManager() { }
        public bool IsAllowedUser(string appId)
        {
            return _ConnectionStringMap.ContainsKey(appId);
        }
        public bool TryGetConnectionString(string appId, out string? connectionString)
        => _ConnectionStringMap.TryGetValue(appId, out connectionString);
    }
}
