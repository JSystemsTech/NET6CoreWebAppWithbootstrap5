using ApplicationInfoServiceAPI.DomainLayer.Models.Data;
using DbFacade.DataLayer.CommandConfig;

namespace ApplicationInfoServiceAPI.DomainLayer
{
    public class AuthenticationTokenServiceDbMethods
    {
        public static readonly IDbCommandMethod<string, ApplicationInfo> GetApplicationInfo
            = AuthenticationTokenServiceDbConnection.GetApplicationInfo.CreateMethod<string, ApplicationInfo>(
            p =>
            {
                p.Add("AppId", p.Factory.Create(m => m));
            },
            v =>
            {
                v.Add(v.Rules.Required(m => m));
            });
    }
}
