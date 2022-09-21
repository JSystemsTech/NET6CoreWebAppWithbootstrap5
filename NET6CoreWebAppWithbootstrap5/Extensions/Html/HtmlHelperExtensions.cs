using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html
{
    public static class HtmlHelperExtensions
    {
        private static string GetExpressionText<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression)
        => ((MemberExpression)expression.Body).Member.Name;

        public static string MetaDataValueFor<TModel, TValue>(this IHtmlHelper<TModel> html,
                                                            Expression<Func<TModel, TValue>> expression,
                                                            Func<ModelMetadata, string> property)
       => property(html.MetaDataFor(expression));
        public static string MetaDataValueFor<TModel, TValue>(this IHtmlHelper<TModel> html,
                                                            ModelMetadata modelMetadata,
                                                            Func<ModelMetadata, string> property)
       => property(modelMetadata);
        public static ModelMetadata MetaDataFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            ModelExpressionProvider expressionProvider = new ModelExpressionProvider(htmlHelper.MetadataProvider);
            return expressionProvider.CreateModelExpression(htmlHelper.ViewData, expression).Metadata;
        }

        public static string ErrorMessageFor<TModel, TValue>(
            this IHtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> expression)
        {
            if (html.ViewData.ModelState.IsValid || html.ViewData.ModelState.Count <= 0)
            {
                return null;
            }
            IEnumerable<string> modelStateErrors = new string[0];
            string key = html.GetExpressionText(expression);
            if (html.ViewData.ModelState is ModelStateDictionary msd && msd.ContainsKey(key) && msd[key] is ModelStateEntry mse)
            {
                modelStateErrors = mse.Errors.Select(e => e.ErrorMessage);
            }
            return modelStateErrors.Count() > 0 ? string.Join(". ", modelStateErrors) : null;

        }
        public static string GetString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
        public static bool HasValidationError<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html.ViewData.ModelState.IsValid || html.ViewData.ModelState.Count <= 0)
            {
                return false;
            }
            IEnumerable<string> modelStateErrors = new string[0];
            string key = html.GetExpressionText(expression);
            if (html.ViewData.ModelState is ModelStateDictionary msd && msd.ContainsKey(key) && msd[key] is ModelStateEntry mse)
            {
                modelStateErrors = mse.Errors.Select(e => e.ErrorMessage);
            }

            return modelStateErrors.Count() > 0;
        }
        public static string UniqueId(this IHtmlHelper htmlHelper)
        => UniqueId();
        public static string UniqueId()
        => $"uid{Guid.NewGuid().ToString().Replace("-", "")}";

        
        public static IHtmlContent Linkify(this IHtmlHelper htmlHelper, string text)
        {
            string result = Regex.Replace(text, @"((http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*)", @"<a href='$1'>$1</a>");

            return new HtmlString(result);
        }
        public static IHtmlContent ToHtmlAttributesString(this IHtmlHelper htmlHelper, object htmlAttributes)
        {
            return new HtmlString(htmlAttributes.ToHtmlAttributesDictionary().ToHtmlAttributeString());
        }
        public static IHtmlContent RenderJsInitData(this IHtmlHelper htmlHelper, object htmlAttributes)
        {
            return new HtmlString($"<div {htmlAttributes.ToHtmlAttributesDictionary().AddUpdateHtmlAttribute("js-init-data", "true").ToHtmlAttributeString()}></div>");
        }
    }
    internal class ImageRequestToken
    {
        public Guid Guid { get; set; }
        public Guid ImageGuid { get; set; }
    }
    public static class ImageLoadHelpers
    {        
        public static IHtmlContent ImageById(this IHtmlHelper htmlHelper, Guid imageGuid)
            => htmlHelper.ImageById(imageGuid, new { });
        public static IHtmlContent ImageById(this IHtmlHelper htmlHelper, Guid imageGuid, object htmlAttributes)
        {
            var htmlAttrs = htmlAttributes.ToHtmlAttributesDictionary();
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext);
            htmlAttrs.AppendHtmlAttribute("src", urlHelper.Action("ShowImage", new { id = imageGuid }) is string src ? src: "");
            return new HtmlString($"<img {htmlAttrs.ToHtmlAttributeString()}/>");
        }

    }
}