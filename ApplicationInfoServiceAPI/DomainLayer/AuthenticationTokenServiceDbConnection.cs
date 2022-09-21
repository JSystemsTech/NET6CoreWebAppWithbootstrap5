using ConnectionStringServiceAPI;
using DbFacade.DataLayer.ConnectionService;
using DbFacade.Factories;
using DbFacade.Services;
using System.Configuration;

namespace ApplicationInfoServiceAPI.DomainLayer
{
    public class AuthenticationTokenServiceDbConnection : SqlConnectionConfig<AuthenticationTokenServiceDbConnection>
    {
        private IConnectionStringManager ConnectionStringManager { get; set; }
        private ConnectionStringInfo? ConnectionStringInfo => ConnectionStringManager.GetConnectionString("AdventureWorksDb");
        public AuthenticationTokenServiceDbConnection(IConnectionStringManager connectionStringManager)
        {
            ConnectionStringManager = connectionStringManager;
            this.Register();
        }
        protected override string GetDbConnectionString() => ConnectionStringInfo is ConnectionStringInfo cs ? cs.ConnectionString : "";
        protected override string GetDbConnectionProvider() => ConnectionStringInfo is ConnectionStringInfo cs ? cs.ProviderName : "";

        protected override async Task<string> GetDbConnectionStringAsync()
        {
            await Task.CompletedTask;
            return GetDbConnectionString();
        }

        protected override async Task<string> GetDbConnectionProviderAsync()
        {
            await Task.CompletedTask;
            return GetDbConnectionProvider();
        }


        public static IDbCommandConfig GetApplicationInfo = Dbo.CreateFetchCommand("GetApplicationInfo", "Get Application Info");
    }
}
