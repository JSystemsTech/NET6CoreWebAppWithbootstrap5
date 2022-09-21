using AuthTokenService.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthTokenService.Configuration
{
    public enum ApplicationEnvironment
    {
        Localhost,
        Development,
        Evaluation,
        Production
    }
    internal class ConfigurationManagerHelper
    {
        public static IDictionary<string, object> GetSettings(string sectionName)
        {
            try
            {
                return ConfigurationManager.GetSection(sectionName) is NameValueCollection settings ?
                    settings.AllKeys.ToDictionary(key => key, key => (object)settings.Get(key)) :
                    new Dictionary<string, object>();
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }
        public static IDictionary<string, object> GetAppSettings()
        {
            try
            {
                return ConfigurationManager.AppSettings.AllKeys.ToDictionary(key => key, key => (object)ConfigurationManager.AppSettings.Get(key));
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }
    }
    public class ApplicationConfiguration
    {
        private static IDictionary<string, object> Settings = ConfigurationManagerHelper.GetAppSettings();
        public static ApplicationEnvironment Environment => Settings.GetSetting<ApplicationEnvironment>("Environment");
        public static string EnvironmentName =>
            Environment == ApplicationEnvironment.Localhost ? "Localhost" :
            Environment == ApplicationEnvironment.Development ? "Development" :
            Environment == ApplicationEnvironment.Evaluation ? "Evaluation" :
            Environment == ApplicationEnvironment.Production ? "Production" :
            "Unknown";
        public static string EncryptionKey => Settings.GetSetting<string>("EncryptionKey");

    }

    public class Saml2TokenConfiguration
    {
        private static IDictionary<string, object> Settings = ConfigurationManagerHelper.GetSettings("saml2TokenSettings");
        public static Uri AudienceUri => new Uri(Settings.GetSetting<string>("AudienceUri"));
        public static string ConfirmationMethod => Settings.GetSetting<string>("ConfirmationMethod");
        public static string Issuer => Settings.GetSetting<string>("Issuer");
        public static string Namespace => Settings.GetSetting<string>("Namespace");
        public static string SubjectName => Settings.GetSetting<string>("SubjectName");
        public static int ValidFor => Settings.GetSetting<int>("ValidFor");

    }
}
