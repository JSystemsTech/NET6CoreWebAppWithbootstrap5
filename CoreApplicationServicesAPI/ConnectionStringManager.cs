using CoreApplicationServicesAPI.DomainLayer;
using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Data.Common;
using System.Data.SqlClient;

namespace CoreApplicationServicesAPI
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
        internal static ConnectionStringInfo Default = new ConnectionStringInfo("", "");
    }
    public class ConnectionStringManager 
    {
        internal static readonly ConnectionStringInfo ApplicationConnectionStringInfo = new ConnectionStringInfo("Data Source=JMAC_LG_PC\\SQLEXPRESS;Initial Catalog=AdventureWorks2019;Persist Security Info=True;User ID=WebAppUser;Password=K$j254a2778899;Trusted_Connection=True", "System.Data.SqlClient");
        
        public bool TryGetConnectionStringInfo(string databaseName, out ConnectionStringInfo? connectionStringInfo)
        {
            connectionStringInfo = BuildSqlConnectionString(databaseName);
            return connectionStringInfo != null;
        }
        
        private ConnectionStringInfo? BuildSqlConnectionString(string databaseName)
        {
            var response = DomainFacade.GetConnectionStringSettings(databaseName, out ConnectionStringSettings? connectionStringSettings);
            if(!response.HasError && connectionStringSettings != null)
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = connectionStringSettings.DataSource;
                builder.InitialCatalog = connectionStringSettings.InitialCatalog;
                builder.PersistSecurityInfo = connectionStringSettings.PersistSecurityInfo;
                builder.UserID = connectionStringSettings.UserID;
                builder.Password = connectionStringSettings.Password;
                return new ConnectionStringInfo($"{builder.ConnectionString}{(connectionStringSettings.TrustedConnection ? ";Trusted_Connection=True" : "")}", "System.Data.SqlClient");
            }
            return null;

        }
    }
}
