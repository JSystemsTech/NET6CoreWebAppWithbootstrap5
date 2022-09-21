using AuthenticationTokenService;
using CoreApplicationServicesAPI;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using NET6CoreWebAppWithbootstrap5.Models;
using NET6CoreWebAppWithbootstrap5.Services.Authentication;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;

namespace NET6CoreWebAppWithbootstrap5.Services
{
    public interface IApplicationServices
    {
        IWebThemeService WebThemeService { get; }
        IViewRenderService ViewRenderService { get; }
        INotificationFactory Notification { get; }
        ApplicationPricipalIdentityManager ApplicationPricipalIdentityManager { get; }
        CoreApplicationServicesAPIClient CoreApplicationServicesAPIClient { get; }
        AuthenticationTokenServiceClient AuthenticationTokenServiceClient { get; }
    }
    internal class ApplicationServices : IApplicationServices
    {
        public IWebThemeService WebThemeService { get; private set; }
        public IViewRenderService ViewRenderService { get; private set; }
        public INotificationFactory Notification { get; private set; }
        public ApplicationPricipalIdentityManager ApplicationPricipalIdentityManager { get; private set; }
        public CoreApplicationServicesAPIClient CoreApplicationServicesAPIClient { get; private set; }
        public AuthenticationTokenServiceClient AuthenticationTokenServiceClient { get; private set; }
        public ApplicationServices(            
            IWebThemeService webThemeService, 
            IViewRenderService viewRenderService,
            INotificationFactory notification,
            ApplicationPricipalIdentityManager applicationPricipalIdentityManager,
            CoreApplicationServicesAPIClient coreApplicationServicesAPIClient,
            AuthenticationTokenServiceClient authenticationTokenServiceClient)
        {
            WebThemeService = webThemeService;
            ViewRenderService = viewRenderService;
            Notification = notification;
            ApplicationPricipalIdentityManager = applicationPricipalIdentityManager;
            CoreApplicationServicesAPIClient = coreApplicationServicesAPIClient;
            AuthenticationTokenServiceClient = authenticationTokenServiceClient;
        }
    }
}
