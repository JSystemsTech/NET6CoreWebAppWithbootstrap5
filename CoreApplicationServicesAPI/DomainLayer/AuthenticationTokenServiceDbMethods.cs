using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.DomainLayer.Models.Parameters;
using CoreApplicationServicesAPI.Models;
using DbFacade.DataLayer.CommandConfig;
using Newtonsoft.Json;
using System.Linq;

namespace CoreApplicationServicesAPI.DomainLayer
{
    public class ApplicationCoreServicesMethods
    {
        public static readonly IDbCommandMethod<(string? appId, string? appCode, bool webAppOnly), ApplicationInfo> GetApplicationInfo
            = ApplicationCoreServicesDbConnection.GetApplicationInfo.CreateMethod<(string? appId, string? appCode, bool webAppOnly), ApplicationInfo>(
            p =>
            {
                p.Add("AppId", p.Factory.Create(m => m.appId));
                p.Add("AppCode", p.Factory.Create(m => m.appCode));
                p.Add("WebAppOnly", p.Factory.Create(m => m.webAppOnly));
            });
        public static readonly IDbCommandMethod<(string appId, ApplicationInfoParameters appInfo)> UpdateApplicationInfo
            = ApplicationCoreServicesDbConnection.UpdateApplicationInfo.CreateMethod<(string appId, ApplicationInfoParameters appInfo)>(
            p =>
            {
                p.Add("AppId", p.Factory.Create(m => m.appId));
                p.Add("Title", p.Factory.Create(m => m.appInfo.Title));
                p.Add("Description", p.Factory.Create(m => m.appInfo.Description));
                p.Add("Theme", p.Factory.Create(m => m.appInfo.Theme));
                p.Add("DefaultRedirectUrl", p.Factory.Create(m => m.appInfo.DefaultRedirectUrl));
                p.Add("SSOUrl", p.Factory.Create(m => m.appInfo.SSOUrl));
                p.Add("SSO_Header", p.Factory.Create(m => m.appInfo.SSO_Header));
                p.Add("RegistrationUrl", p.Factory.Create(m => m.appInfo.RegistrationUrl));
                p.Add("RequireRegistration", p.Factory.Create(m => m.appInfo.RequireRegistration));
                p.Add("LogoFileName", p.Factory.Create(m => m.appInfo.LogoFileName));
                p.Add("LogoFileContentType", p.Factory.Create(m => m.appInfo.LogoFileContentType));
                p.Add("LogoFileData", p.Factory.Create(m => m.appInfo.LogoFileData));
            },
            v => {
                v.Add(v.Rules.Required(m => m.appId));
            }); 
        public static readonly IDbCommandMethod<(string appId, ApplicationAPIConfig config)> UpdateApplicationInfoApplicationAPIConfig
            = ApplicationCoreServicesDbConnection.UpdateApplicationInfoApplicationAPIConfig.CreateMethod<(string appId, ApplicationAPIConfig config)>(
            p =>
            {
                p.Add("AppId", p.Factory.Create(m => m.appId));
                p.Add("Title", p.Factory.Create(m => JsonConvert.SerializeObject(m.config)));
            },
            v => {
                v.Add(v.Rules.Required(m => m.appId));
            });

        public static readonly IDbCommandMethod<string, ConnectionStringSettings> GetConnectionStringSettings
            = ApplicationCoreServicesDbConnection.GetConnectionStringSettings.CreateMethod<string, ConnectionStringSettings>(
            p =>
            {
                p.Add("InitialCatalog", p.Factory.Create(m => m));
            },
            v =>
            {
                v.Add(v.Rules.Required(m => m));
            });
    }
}
