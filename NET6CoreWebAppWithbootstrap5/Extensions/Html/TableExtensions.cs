using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html
{
    public static class TableExtensions
    {
        public static IHtmlContent Table(this IHtmlHelper htmlHelper, object htmlAttributes)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendClass("table");
            return new HtmlString($"<table {attrs.ToHtmlAttributeString()}></table>");
        }
    }
}