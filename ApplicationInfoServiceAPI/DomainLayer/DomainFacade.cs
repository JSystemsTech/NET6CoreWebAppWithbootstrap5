using ApplicationInfoServiceAPI.DomainLayer.Models.Data;
using DbFacade.DataLayer.Models;

namespace ApplicationInfoServiceAPI.DomainLayer
{
    public class DomainFacade
    {
        public static ApplicationInfo? GetApplicationInfo(string name)
        {
            var response = AuthenticationTokenServiceDbMethods.GetApplicationInfo.Execute(name);
            return !response.HasError ? response.FirstOrDefault() : null;
        }
        public static async Task<ApplicationInfo?> GetApplicationInfoAsync(string name)
        {
            var response = await AuthenticationTokenServiceDbMethods.GetApplicationInfo.ExecuteAsync(name);
            return !response.HasError ? response.FirstOrDefault() : null;
        }
    }
}
