using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.DomainLayer.Models.Parameters;
using CoreApplicationServicesAPI.Models;
using DbFacade.DataLayer.Models;

namespace CoreApplicationServicesAPI.DomainLayer
{
    public class DomainFacade
    {
        public static IDbResponse<ApplicationInfo> GetApplicationInfo(string appId, out ApplicationInfo? applicationInfo)
        {
            var response = ApplicationCoreServicesMethods.GetApplicationInfo.Execute((appId, null, false));
            applicationInfo =!response.HasError ? response.FirstOrDefault() : null;
            return response;
        }
        public static async Task<ApplicationInfo?> GetApplicationInfoAsync(string appId)
        {
            var response = await ApplicationCoreServicesMethods.GetApplicationInfo.ExecuteAsync((appId, null, false));
            return !response.HasError ? response.FirstOrDefault() : null;
        }
        public static IDbResponse<ApplicationInfo> GetWebApplicationInfo(string? appCode = null)
        => ApplicationCoreServicesMethods.GetApplicationInfo.Execute((null, appCode, true));
        public static async Task<IDbResponse<ApplicationInfo>> GetWebApplicationInfoAsync(string? appCode = null)
        => await ApplicationCoreServicesMethods.GetApplicationInfo.ExecuteAsync((null, appCode, true));
        public static IDbResponse<ConnectionStringSettings> GetConnectionStringSettings(string name, out ConnectionStringSettings? connectionStringSettings)
        {
            var response = ApplicationCoreServicesMethods.GetConnectionStringSettings.Execute(name);
            connectionStringSettings = !response.HasError ? response.FirstOrDefault() : null;
            return response;
        }
        public static async Task<ConnectionStringSettings?> GetConnectionStringSettingsAsync(string name)
        {
            var response = await ApplicationCoreServicesMethods.GetConnectionStringSettings.ExecuteAsync(name);
            return !response.HasError ? response.FirstOrDefault() : null;
        }

        public static IDbResponse UpdateApplicationInfo(string appId, ApplicationInfoParameters applicationInfo)
        => ApplicationCoreServicesMethods.UpdateApplicationInfo.Execute((appId, applicationInfo));
        public static async Task<IDbResponse> UpdateApplicationInfoAsync(string appId, ApplicationInfoParameters applicationInfo)
        => await ApplicationCoreServicesMethods.UpdateApplicationInfo.ExecuteAsync((appId, applicationInfo));

        public static IDbResponse UpdateApplicationInfoRegisterUserFormConfig(string appId, RegisterUserFormConfig config)
        => ApplicationCoreServicesMethods.UpdateApplicationInfoRegisterUserFormConfig.Execute((appId, config));
        public static async Task<IDbResponse> UpdateApplicationInfoRegisterUserFormConfigAsync(string appId, RegisterUserFormConfig config)
        => await ApplicationCoreServicesMethods.UpdateApplicationInfoRegisterUserFormConfig.ExecuteAsync((appId, config));
    }
}
