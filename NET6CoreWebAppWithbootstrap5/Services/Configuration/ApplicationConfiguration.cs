using Aspose.Tasks.Connectivity;
using NET6CoreWebAppWithbootstrap5.Models;
using System;

namespace NET6CoreWebAppWithbootstrap5.Services.Configuration
{
    public static class ApplicationConfiguration
    {
        public static ApplicationSettings ApplicationSettings = new ApplicationSettings();
        public static NotificationOptions NotificationOptions = new NotificationOptions();
        public static OpenAPIUrlsSettings OpenAPIUrlsSettings = new OpenAPIUrlsSettings();

        internal static ConfigurationManager UseStaticApplicationConfiguration(this ConfigurationManager configuration)
        {
            configuration.GetSection("ApplicationSettings").Bind(ApplicationSettings);
            configuration.GetSection("NotificationOptions").Bind(NotificationOptions);
            configuration.GetSection("OpenAPIUrls").Bind(OpenAPIUrlsSettings);
            return configuration;
        }
    }
    public class OpenAPIUrlsSettings
    {
        public string AuthenticationTokenService { get; set; } = "";
        public string CoreApplicationServicesAPI { get; set; } = "";
    }
    public enum ApplicationEnvironment
    {
        Localhost,
        Development,
        Evaluation,
        Production,
        Unknown
    }
    public class ApplicationSettings
    {
        private ApplicationEnvironment TryGetEnvironment()
        {
            try
            {
                return Enum.Parse<ApplicationEnvironment>(EnvironmentName);
            }
            catch
            {
                return ApplicationEnvironment.Unknown;
            }
        }
        public ApplicationEnvironment Environment => TryGetEnvironment();
        public string EnvironmentName { get; set; } = "Localhost"; 
        public string AppId { get; set; } = "";
        public bool ShowEnvironmentName { get; set; } = true;
        public string DefaultTheme { get; set; } = "Base";
        public string ApplicationName { get; set; } = "";
        public string ApplicationDescription { get; set; } = "";
        public string AsposeLicensePath { get; set; } = "";
        public string LastLoginDateFormat { get; set; } = "MM/dd/yyyy HH:mm";

        //public static ApplicationEnvironment Environment => AppSettings.GetSetting<ApplicationEnvironment>("Environment");
        //public static string EnvironmentName =>
        //    Environment == ApplicationEnvironment.Localhost ? "Localhost" :
        //    Environment == ApplicationEnvironment.Development ? "Development" :
        //    Environment == ApplicationEnvironment.Evaluation ? "Evaluation" :
        //    Environment == ApplicationEnvironment.Production ? "Production" :
        //    "Unknown";
        //public static bool ShowEnvironmentName => AppSettings.GetSetting("ShowEnvironmentName", true);
        //public static IEnumerable<string> Themes => AppSettings.GetEnumerableSetting<string>("Themes");
        //public static string DefaultTheme => AppSettings.GetSetting<string>("DefaultTheme");
        //public static string ApplicationName => AppSettings.GetSetting<string>("ApplicationName");
        //public static string ApplicationDescription => AppSettings.GetSetting<string>("ApplicationDescription");
        //public static string EncryptionKey => AppSettings.GetSetting<string>("EncryptionKey");
        //public static string AuthenticationCookieName => AppSettings.GetSetting<string>("AuthenticationCookieName");
        //public static string LogoutUrl => AppSettings.GetSetting<string>("LogoutUrl");
        //public static string AsposeLicensePath => AppSettings.GetSetting<string>("AsposeLicensePath");
        //public static string MainSQLDbConnectionKey => AppSettings.GetSetting<string>("MainSQLDbConnectionKey");
        //public static string ProxyRequestParam => AppSettings.GetSetting<string>("ProxyRequestParam");
        //public static string ProxyEndParam => AppSettings.GetSetting<string>("ProxyEndParam");
        //public static string DarkModeChangeParam => AppSettings.GetSetting<string>("DarkModeChangeParam");
        //public static string LastLoginDateFormat => AppSettings.GetSetting("LastLoginDateFormat", "MM/dd/yyyy HH:mm");
        //public static string ExternalLoginTokenParam => AppSettings.GetSetting<string>("ExternalLoginTokenParam");
    }
}
