using Microsoft.AspNetCore.Mvc.Rendering;
using NET6CoreWebAppWithbootstrap5.Extensions.Html;
using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace NET6CoreWebAppWithbootstrap5.Services
{
    public interface IWebThemeService
    {
        string? GetTheme(string name, bool isDark = false);
        SelectList<Theme> GetThemeList(string selected);
    }
    public class Theme
    {
        public string? Name { get; internal set; }
        internal string? Light { get; set; }
        internal string? Dark { get; set; }

        public string? GetPath( bool isDark = false) => isDark && !string.IsNullOrWhiteSpace(Dark)? Dark: Light;

        public Theme(string name)
        {
            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            Name = myTI.ToTitleCase(name);
            Light = $"~/css/theme/{name}/light.css";
            Dark = $"~/css/theme/{name}/dark.css";
        }
    }
    internal class WebThemeService: IWebThemeService
    {
        private IEnumerable<Theme> Themes { get; set; }
        private static Theme DefaultTheme = new Theme("base");
        public string? GetTheme(string name, bool isDark = false) => Themes.FirstOrDefault(t => t.Name == name) is Theme theme ? theme.GetPath(isDark): DefaultTheme.GetPath(isDark);

        public SelectList<Theme> GetThemeList(string selected) => Themes.ToSelectList(m => m.Name, m => m.Name, selected);
        public WebThemeService(IWebHostEnvironment env)
        {
            string path = $"{env.WebRootPath}\\css\\theme";
            if (!Directory.Exists(path))
            {
                throw new Exception("failed to locate theme directory");
            }
            Themes = Directory.GetDirectories(path).Select(dirPath=> new Theme(Path.GetFileName(dirPath)));
        }
    }
}
