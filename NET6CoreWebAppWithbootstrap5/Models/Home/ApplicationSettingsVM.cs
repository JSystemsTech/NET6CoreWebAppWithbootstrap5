using CoreApplicationServicesAPI;
using Microsoft.AspNetCore.Mvc.Rendering;
using NET6CoreWebAppWithbootstrap5.Extensions.Html;
using NET6CoreWebAppWithbootstrap5.Services;
using System.ComponentModel.DataAnnotations;

namespace NET6CoreWebAppWithbootstrap5.Models.Home
{
    public class ApplicationSettingsVM
    {
        [Display(Name = "Title")]
        [Required]
        public string Title { get; set; } = "";
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; } = "";
        [Display(Name = "Theme")]
        [Required]
        public string Theme { get; set; } = "";
        [Display(Name = "Default Redirect Url")]
        [Required]
        public string DefaultRedirectUrl { get; set; } = "";
        [Display(Name = "SSO Url")]
        public string? SsoUrl { get; set; }
        [Display(Name = "Registration Url")]
        public string? RegistrationUrl { get; set; }
        [Display(Name = "Require Registration")]
        public bool RequireRegistration { get; set; }
        
        [Display(Name = "Application Logo")]
        public IFormFile? ApplicationLogo { set; get; }

        public string? LogoFileName { get; set; } 

        public string? LogoFileContentType { get; set; } 

        public byte[]? LogoFileData { get; set; }
        [Display(Name = "SSO Header")]
        [Required]
        public string SsO_Header { get; set; } = "";
        public SelectList<Theme> ThemeSelectList { get; set; } 
        public ApplicationSettingsVM() { ThemeSelectList = (new Theme[0]).ToSelectList(m=>m.Name, m => m.Name); }
        public ApplicationSettingsVM(ApplicationInfo appInfo, SelectList<Theme> themeSelectList) {
            Title = appInfo.Title;
            Description = appInfo.Description;
            Theme = appInfo.Theme;
            DefaultRedirectUrl = appInfo.DefaultRedirectUrl;
            SsoUrl = appInfo.SsoUrl;
            SsO_Header = appInfo.SsO_Header;
            RegistrationUrl = appInfo.RegistrationUrl;
            RequireRegistration = appInfo.RequireRegistration;
            LogoFileName = appInfo.LogoFileName;
            LogoFileContentType = appInfo.LogoFileContentType;
            LogoFileData = appInfo.LogoFileData;
            ThemeSelectList =   themeSelectList;

        }
    }
}
