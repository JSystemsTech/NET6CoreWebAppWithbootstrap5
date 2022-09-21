using CoreApplicationServicesAPI.Models;
using DbFacade.DataLayer.Models;
using Newtonsoft.Json;
using System.Security.Principal;

namespace CoreApplicationServicesAPI.DomainLayer.Models.Data
{
    public class ApplicationInfo : DbDataModel
    {
        public string AppId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Theme { get; set; } = "";
        public string Type { get; set; } = "";
        public string DefaultRedirectUrl { get; set; } = ""; 
        public string? SSOUrl { get; set; }
        public string? RegistrationUrl { get; set; }
        public bool RequireRegistration { get; set; }

        public string? LogoFileName { get; set; }
        public string? LogoFileContentType { get; set; }
        public byte[]? LogoFileData { get; set; }

        public string SSO_Header { get; set; } = "";
        public ApplicationAPIConfig? ApplicationAPIConfig { get; set; }

        internal static ApplicationInfo Default = new ApplicationInfo();
        protected override void Init()
        {
            AppId = GetColumn("AppId", "");
            Title = GetColumn("Title", "");
            Description = GetColumn("Description", "");
            Theme = GetColumn("Theme", "");
            Type = GetColumn("Type", "");
            DefaultRedirectUrl = GetColumn("DefaultRedirectUrl", "");
            SSOUrl = GetColumn<string?>("SSOUrl");
            RegistrationUrl = GetColumn<string?>("RegistrationUrl");
            RequireRegistration = GetColumn<bool>("RequireRegistration");

            LogoFileName = GetColumn<string?>("LogoFileName");
            LogoFileContentType = GetColumn<string?>("LogoFileContentType");
            LogoFileData = GetColumn<byte[]?> ("LogoFileData");
            SSO_Header = GetColumn("SSO_Header", "");

            string json  = GetColumn("ApplicationAPIConfig", "");
            if (!string.IsNullOrWhiteSpace(json))
            {
                ApplicationAPIConfig = JsonConvert.DeserializeObject<ApplicationAPIConfig>(json);
            }
            
        }
        protected override async Task InitAsync()
        {
            AppId = await GetColumnAsync("AppId", "");
            Title = await GetColumnAsync("Title", "");

            Theme = await GetColumnAsync("Theme", "");
            Description = await GetColumnAsync("Description", "");
            Type = await GetColumnAsync("Type", "");
            DefaultRedirectUrl = await GetColumnAsync("DefaultRedirectUrl", "");
            SSOUrl = await GetColumnAsync<string?>("SSOUrl");
            RegistrationUrl = await GetColumnAsync<string?>("RegistrationUrl");
            RequireRegistration = await GetColumnAsync<bool>("RequireRegistration");

            LogoFileName = await GetColumnAsync<string?>("LogoFileName");
            LogoFileContentType = await GetColumnAsync<string?>("LogoFileContentType");
            LogoFileData = await GetColumnAsync<byte[]?>("LogoFileData");
            SSO_Header = await GetColumnAsync("SSO_Header", "");

            string json = await GetColumnAsync("ApplicationAPIConfig", "");
            if (!string.IsNullOrWhiteSpace(json))
            {
                ApplicationAPIConfig = JsonConvert.DeserializeObject<ApplicationAPIConfig>(json);
            }
        }

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public string? ErrorMessage { get; protected set; }
        public static ApplicationInfo Error(string errorMessage)
        => new ApplicationInfo()
        {
            ErrorMessage = errorMessage
        };
    }
    public class ConnectionStringSettings : DbDataModel
    {
        public string DataSource { get; set; } = "";
        public string InitialCatalog { get; set; } = "";
        public bool PersistSecurityInfo { get; set; }
        public string UserID { get; set; } = "";
        public string Password { get; set; } = "";
        public bool TrustedConnection { get; set; }

        protected override void Init()
        {
            DataSource = GetColumn("DataSource", "");
            InitialCatalog = GetColumn("InitialCatalog", "");
            PersistSecurityInfo = GetColumn("PersistSecurityInfo", true);
            UserID = GetColumn("UserID", "");
            Password = GetColumn("Password", "");
            TrustedConnection = GetColumn("TrustedConnection", true);
        }
        protected override async Task InitAsync()
        {
            DataSource = await GetColumnAsync("DataSource", "");
            InitialCatalog = await GetColumnAsync("InitialCatalog", "");
            PersistSecurityInfo = await GetColumnAsync("PersistSecurityInfo", true);
            UserID = await GetColumnAsync("UserID", "");
            Password = await GetColumnAsync("Password", "");
            TrustedConnection = await GetColumnAsync("TrustedConnection", true);
        }
    }
}
