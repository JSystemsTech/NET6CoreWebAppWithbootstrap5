using Aspose.Diagram;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NET6CoreWebAppWithbootstrap5.Extensions.Html;
using NET6CoreWebAppWithbootstrap5.Models;
using System.Linq.Expressions;
using Westwind.AspNetCore.Markdown;

namespace NET6CoreWebAppWithbootstrap5.Extensions
{
    public static class HtmlStringExtensions
    {
        public static HtmlString SafeMarkdown<TModel>(this IHtmlHelper<TModel> html, Func<TModel, string> handler, bool sanitizeHtml = true, bool usePragmaLines = false, bool forceReload = false)
        {
            return Markdown.ParseHtmlString(handler(html.ViewData.Model), usePragmaLines, forceReload, sanitizeHtml);
        }
        public static IDictionary<string, object?> ToHtmlAttributesDictionary(this object htmlAttributes)
            => HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        public static IDictionary<string, object?> AppendHtmlAttribute(this IDictionary<string, object?> htmlAttributes, string key, string value)
        {
            if (htmlAttributes.TryGetValue(key, out object? currentValue))
            {
                if (currentValue is string currentValueStr)
                {
                    htmlAttributes[key] = $"{currentValueStr} {value}";
                }
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
            return htmlAttributes;
        }
        public static IDictionary<string, object?> AddUpdateHtmlAttribute(this IDictionary<string, object?> htmlAttributes, string key, string value)
        {
            if (htmlAttributes.ContainsKey(key))
            {
                htmlAttributes[key] = value;
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
            return htmlAttributes;
        }
        public static bool HasClass(this IDictionary<string, object?> htmlAttributes, string value)
            => htmlAttributes.TryGetValue("class", out object? currentValue) && currentValue is string currentValueStr && currentValueStr.HasClass(value);
        public static bool HasClass(this string classes, string value)
            => classes.Split(' ').Contains(value);
        public static IDictionary<string, object?> AppendClass(this IDictionary<string, object?> htmlAttributes, string value)
        {
            foreach (string cls in value.Split(' '))
            {
                htmlAttributes = !htmlAttributes.HasClass(cls) ? htmlAttributes.AppendHtmlAttribute("class", cls) : htmlAttributes;
            }
            return htmlAttributes;
        }
        public static IDictionary<string, object?> MergeAttributes(this IDictionary<string, object?> htmlAttributes, IDictionary<string, object?> htmlAttributes2)
        {
            foreach (var kv in htmlAttributes2)
            {
                if (htmlAttributes.ContainsKey(kv.Key))
                {
                    htmlAttributes.Add(kv.Key, kv.Value);
                }
                else
                {
                    htmlAttributes[kv.Key] = kv.Value;
                }
            }
            return htmlAttributes;
        }

        public static string FlattenToString(this IDictionary<string, object?> htmlAttributes)
        {
            string flatAttrValues = string.Join(' ', htmlAttributes.Select(kv => { 

                var value = kv.Value is bool b ? b.ToString().ToLower() : kv.Value.ToString();
                return $"{kv.Key}=\"{value}\"";
                
                }));
            return flatAttrValues;
        }

        

        

        public static RouteValueDictionary ToRouteValueDictionary(this object routeValues)
            => new RouteValueDictionary(routeValues);
        public static string ToHtmlAttributeString(this IDictionary<string, object?> htmlAttributes)
            => string.Join(" ", htmlAttributes.Select(attr => $"{attr.Key}=\"{attr.Value}\""));

        public static byte[] ToArray(this IFormFile file)
        {
            byte[] data;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                data = ms.ToArray();
            }
            return data;
        }
    }
}
