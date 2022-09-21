using Microsoft.AspNetCore.Mvc.Rendering;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using System.Reflection;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html
{
    public static class ApplicationVersionExtensions
    {
        internal static ApplicationSettings AppConfig => ApplicationConfiguration.ApplicationSettings;
        public static string? ApplicationVersion(this IHtmlHelper htmlHelper)
        => Assembly.GetExecutingAssembly().GetName().Version is Version version ? version.ToString() : null;
        public static string ApplicationName(this IHtmlHelper htmlHelper)
        => AppConfig.ApplicationName;
        public static string ApplicationDescription(this IHtmlHelper htmlHelper)
        => AppConfig.ApplicationDescription;
        public static string ApplicationEnvironment(this IHtmlHelper htmlHelper)
        => AppConfig.ShowEnvironmentName ? AppConfig.EnvironmentName : "";

        public static string ApplicationNameEnvironment(this IHtmlHelper htmlHelper)
        {
            string env = AppConfig.ShowEnvironmentName ? $" - {AppConfig.EnvironmentName}" : "";
            return $"{htmlHelper.ApplicationName()}{env}";
        }
        public static string ApplicationNameVersionEnvironment(this IHtmlHelper htmlHelper)
        {
            string env = AppConfig.ShowEnvironmentName ? $" - {AppConfig.EnvironmentName}" : "";
            return $"{htmlHelper.ApplicationName()}{env} v{htmlHelper.ApplicationVersion()}";
        }

    }
}