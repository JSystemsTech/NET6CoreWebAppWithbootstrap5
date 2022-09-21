using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html.Bootstrap
{
    public static class FormControlSelectExtensions
    {
        private static IDictionary<string, object?> AppendFormControlClass(this IDictionary<string, object?> htmlAttributes)
               => htmlAttributes.AppendClass("form-select");
        public static IHtmlContent FormControlDropDownListFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList)
        => htmlHelper.FormControlDropDownListFor(expression, selectList, new { });
        public static IHtmlContent FormControlDropDownListFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList,
            object htmlAttributes)
        {
            var htmlAttrs = htmlAttributes.ToHtmlAttributesDictionary().AppendFormControlClass();
            return htmlHelper.ExtendedDropDownListFor(expression, selectList, htmlAttrs);
        }

        public static IHtmlContent FormControlListBoxFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList)
        => htmlHelper.FormControlListBoxFor(expression, selectList, new { });
        public static IHtmlContent FormControlListBoxFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListStandardItem> selectList,
            object htmlAttributes)
        {
            var htmlAttrs = htmlAttributes.ToHtmlAttributesDictionary().AppendFormControlClass();
            return htmlHelper.ExtendedListBoxFor(expression, selectList, htmlAttrs);
        }
    }
}