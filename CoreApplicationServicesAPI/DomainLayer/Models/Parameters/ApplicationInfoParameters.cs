using DbFacade.DataLayer.Models;

namespace CoreApplicationServicesAPI.DomainLayer.Models.Parameters
{
    public class ApplicationInfoParameters : DbDataModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; } 
        public string? Theme { get; set; } 
        public string? Type { get; set; } 
        public string? DefaultRedirectUrl { get; set; } 
        public string? SSOUrl { get; set; }
        public string? RegistrationUrl { get; set; }
        public bool RequireRegistration { get; set; }

        public string? LogoFileName { get; set; }
        public string? LogoFileContentType { get; set; }
        public byte[]? LogoFileData { get; set; }

        public string? SSO_Header { get; set; }

        public string? ApplicationAPIUrl { get; set; }
    }
}
