using DbFacade.DataLayer.ConnectionService;
using DbFacade.Services;

namespace CoreApplicationServicesAPI.DomainLayer
{
    public class ApplicationCoreServicesDbConnection : SqlConnectionConfig<ApplicationCoreServicesDbConnection>
    {
        protected override string GetDbConnectionString() => ConnectionStringManager.ApplicationConnectionStringInfo.ConnectionString;
        protected override string GetDbConnectionProvider() => ConnectionStringManager.ApplicationConnectionStringInfo.ProviderName;

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
        public static IDbCommandConfig UpdateApplicationInfo = Dbo.CreateFetchCommand("UpdateApplicationInfo", "Update Application Info");
        public static IDbCommandConfig UpdateApplicationInfoApplicationAPIConfig = Dbo.CreateFetchCommand("UpdateApplicationInfo_ApplicationAPIConfig", "Update Application Info Application API Config");
        public static IDbCommandConfig GetConnectionStringSettings = Dbo.CreateFetchCommand("GetConnectionStringSettings", "Get Connection String Settings");
    }
}
