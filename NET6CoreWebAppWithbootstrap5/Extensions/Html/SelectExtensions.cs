using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Web;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html
{
    public class SelectListStandardItem : SelectListItem
    {
        public static readonly string StandardValueField = "Value";
        public static readonly string StandardTextField = "Text";
        public static readonly string StandardGroupField = "Group.Name";
        public object? HtmlAttributes { get; set; }
        public SelectListStandardItem() { }
        public static SelectListStandardItem Create<T>(T data, Func<T, object> dataValueSelector, Func<T, object> dataTextSelector)
            => new SelectListStandardItem() { Text = dataTextSelector(data).ToString(), Value = dataValueSelector(data).ToString() };

        public static SelectListStandardItem Create<T>(T data, Func<T, object> dataValueSelector, Func<T, object> dataTextSelector, Func<T, object> dataGroupSelector)
            => new SelectListStandardItem() { Text = dataTextSelector(data).ToString(), Value = dataValueSelector(data).ToString(), Group = new SelectListGroup() { Name = dataGroupSelector(data).ToString() } };

    }
    public static class MultiSelectListExtensions
    {

        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object>? selectedValues = null)
            where T : class
        => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValues);
        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object> disabledValues)
            where T : class
        => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValues, disabledValues);

        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object>? selectedValues = null,
            Func<T, object>? dataGroupSelector = null)
        => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, dataGroupSelector);
        public static MultiSelectList<T> ToMultiSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object>? disabledValues,
            Func<T, object>? dataGroupSelector = null)
            => new MultiSelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, disabledValues, dataGroupSelector);
        internal static IEnumerable<SelectListStandardItem> ToSelectListStandardItems<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object>? selectedValues,
            IEnumerable<object>? disabledValues,
            Func<T, object>? dataGroupSelector = null)
       => data.Select(item => {
           SelectListStandardItem selectListItem =
               dataGroupSelector != null ? SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector, dataGroupSelector) :
               SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector);
           selectListItem.HtmlAttributes = htmlAttributeSelector != null ? htmlAttributeSelector(item) : new { };
           selectListItem.Selected = selectedValues != null && selectedValues.Any(sv => sv.ToString() == selectListItem.Value);
           selectListItem.Disabled = disabledValues != null && disabledValues.Any(sv => sv.ToString() == selectListItem.Value);
           return selectListItem;
       });
        internal static IEnumerable<SelectListStandardItem> ToSelectListStandardItems<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object? selectedValue,
            IEnumerable<object>? disabledValues,
            Func<T, object>? dataGroupSelector = null)
       => data.Select(item => {
           SelectListStandardItem selectListItem =
               dataGroupSelector != null ? SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector, dataGroupSelector) :
               SelectListStandardItem.Create(item, dataValueSelector, dataTextSelector);
           selectListItem.HtmlAttributes = htmlAttributeSelector != null ? htmlAttributeSelector(item) : new { };
           selectListItem.Selected = selectedValue != null && selectedValue.ToString() == selectListItem.Value;
           selectListItem.Disabled = disabledValues != null && disabledValues.Any(sv => sv.ToString() == selectListItem.Value);
           return selectListItem;
       });
    }
    public class MultiSelectList<T> : MultiSelectList, IEnumerable<SelectListStandardItem>
    {
        private IEnumerable<SelectListStandardItem> ExtendedItems { get; set; }
        IEnumerator<SelectListStandardItem> IEnumerable<SelectListStandardItem>.GetEnumerator()
        => ExtendedItems.GetEnumerator();
        public MultiSelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object>? selectedValues = null)
            : this(data, htmlAttributeSelector, dataValueSelector, dataValueSelector, selectedValues is IEnumerable<object> sVals ? sVals : new object[0]) { }
        public MultiSelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            IEnumerable<object> selectedValues,
            IEnumerable<object> disabledValues)
            : this(data, htmlAttributeSelector, dataValueSelector, dataValueSelector, selectedValues, disabledValues) { }

        public MultiSelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            IEnumerable<object>? selectedValues = null,
            Func<T, object>? dataGroupSelector = null)
        : this(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues is IEnumerable<object> sVals ? sVals: new object[0], null, dataGroupSelector) { }


        public MultiSelectList(
                    IEnumerable<T> data,
                    Func<T, object> htmlAttributeSelector,
                    Func<T, object> dataValueSelector,
                    Func<T, object> dataTextSelector,
                    IEnumerable<object> selectedValues,
                    IEnumerable<object>? disabledValues,
                    Func<T, object>? dataGroupSelector = null) :
                    base(data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, disabledValues, dataGroupSelector), SelectListStandardItem.StandardValueField, SelectListStandardItem.StandardTextField, selectedValues, SelectListStandardItem.StandardGroupField )
        {
            ExtendedItems = data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValues, disabledValues, dataGroupSelector);
        }

        
    }

    public static class SelectListExtensions
    {


        public static SelectList<T> ToSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            object? selectedValue = null)
        => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValue is object sVal ? sVal : "");
        public static SelectList<T> ToSelectList<T>(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            object selectedValue,
            IEnumerable<object> disabledValues)
        => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, selectedValue, disabledValues);

        public static SelectList<T> ToSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object? selectedValue = null,
            Func<T, object>? dataGroupSelector = null)
        => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue is object sVal ? sVal : "", dataGroupSelector);
        public static SelectList<T> ToSelectList<T>(
            this IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            IEnumerable<object>? disabledValues,
            Func<T, object>? dataGroupSelector = null)
            => new SelectList<T>(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, disabledValues, dataGroupSelector);
    }
    public class SelectList<T> : SelectList, IEnumerable<SelectListStandardItem>
    {
        private IEnumerable<SelectListStandardItem> ExtendedItems { get; set; }
        IEnumerator<SelectListStandardItem> IEnumerable<SelectListStandardItem>.GetEnumerator()
        => ExtendedItems.GetEnumerator();
        public SelectList(
                IEnumerable<T> data,
                Func<T, object> htmlAttributeSelector,
                Func<T, object> dataValueSelector,
                object? selectedValue = null)
                : this(data, htmlAttributeSelector, dataValueSelector, selectedValue is object sVal? sVal: "", null) { }
        public SelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            object selectedValue,
            IEnumerable<object>? disabledValues = null)
                : this(data, htmlAttributeSelector, dataValueSelector, dataValueSelector, selectedValue, disabledValues) { }
        public SelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            Func<T, object>? dataGroupSelector = null)
            : this(data, htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, null, dataGroupSelector) { }
        public SelectList(
            IEnumerable<T> data,
            Func<T, object> htmlAttributeSelector,
            Func<T, object> dataValueSelector,
            Func<T, object> dataTextSelector,
            object selectedValue,
            IEnumerable<object>? disabledValues,
            Func<T, object>? dataGroupSelector = null)
            : base(data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, disabledValues, dataGroupSelector), SelectListStandardItem.StandardValueField, SelectListStandardItem.StandardTextField, selectedValue, SelectListStandardItem.StandardGroupField)
        {
            ExtendedItems = data.ToSelectListStandardItems(htmlAttributeSelector, dataValueSelector, dataTextSelector, selectedValue, disabledValues, dataGroupSelector);
        }
    }
    

    public static class ExtendedSelectExtensions
    {
        internal static string?[]? GetModelStateValueArray(this IHtmlHelper htmlHelper, string key)
        {
            ModelStateEntry? modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState is ModelStateEntry ms && ms.RawValue != null)
                {
                    return ((IEnumerable)ms.RawValue).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
                }
            }
            return null;
        }
        internal static string? GetModelStateValue(this IHtmlHelper htmlHelper, string key)
        {
            ModelStateEntry? modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState is ModelStateEntry ms && ms.RawValue != null)
                {
                    return ms.RawValue.ToString();
                }
            }
            return null;
        }

        public static IHtmlContent ExtendedDropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListStandardItem> selectList, IDictionary<string, object?> htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
        var metadata = htmlHelper.MetaDataFor(expression);
            ModelExpressionProvider expressionProvider = new ModelExpressionProvider(htmlHelper.MetadataProvider);
            return SelectInternal(htmlHelper, metadata, expressionProvider.GetExpressionText(expression), selectList,
                false /* allowMultiple */, htmlAttributes);
        }
        public static IHtmlContent ExtendedListBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListStandardItem> selectList, IDictionary<string, object?> htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            ModelExpressionProvider expressionProvider = new ModelExpressionProvider(htmlHelper.MetadataProvider);
            var metadata = expressionProvider.CreateModelExpression(htmlHelper.ViewData, expression).Metadata;

            return SelectInternal(htmlHelper, metadata, expressionProvider.GetExpressionText(expression), selectList,
                true /* allowMultiple */, htmlAttributes);
        }
        private static IHtmlContent SelectInternal(this IHtmlHelper htmlHelper, ModelMetadata metadata, string name,
            IEnumerable<SelectListStandardItem> selectList, bool allowMultiple,
            IDictionary<string, object?> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentException("No name");

            if (selectList == null)
                throw new ArgumentException("No selectlist");

            object? defaultValue = (allowMultiple)
                ? htmlHelper.GetModelStateValueArray(fullName)
                : htmlHelper.GetModelStateValue(fullName);

            // If we haven't already used ViewData to get the entire list of items then we need to
            // use the ViewData-supplied value before using the parameter-supplied value.
            if (defaultValue == null)
                defaultValue = htmlHelper.ViewData.Eval(fullName);

            if (defaultValue != null)
            {
                IEnumerable defaultValues = (allowMultiple) && defaultValue is IEnumerable arr ? arr : new[] { defaultValue };
                IEnumerable<string> values = from object value in defaultValues
                                             select Convert.ToString(value, CultureInfo.CurrentCulture);
                HashSet<string> selectedValues = new HashSet<string>(values, StringComparer.OrdinalIgnoreCase);
                List<SelectListStandardItem> newSelectList = new List<SelectListStandardItem>();

                foreach (SelectListStandardItem item in selectList)
                {
                    item.Selected = (item.Value != null)
                        ? selectedValues.Contains(item.Value)
                        : selectedValues.Contains(item.Text);
                    newSelectList.Add(item);
                }
                selectList = newSelectList;
            }

            // Convert each ListItem to an <option> tag
            string optionsString = "";

            if(selectList.All(m=> m.Group != null))
            {
                string optgroups = string.Join("", selectList.GroupBy(m => m.Group.Name).Select(g => {
                    string options = string.Join("", g.Select(m => ListItemToOption(m)));
                    return $"<optgroup label=\"{g.Key}\">{options}</optgroup";
                }));
                optionsString = $"{optionsString}{optgroups}";
            }
            else
            {
                string options = string.Join("", selectList.Select(m => ListItemToOption(m)));
                optionsString = $"{optionsString}{options}";
            }
            
            htmlAttributes.AddUpdateHtmlAttribute("name", fullName);
            if (!htmlAttributes.ContainsKey("id"))
            {
                htmlAttributes.Add("id", fullName);
            }
            if (allowMultiple)
            {
                htmlAttributes.AddUpdateHtmlAttribute("multiple", "multiple");
            }

            // If there are any errors for a named field, we add the css attribute.
            ModelStateEntry? modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    htmlAttributes.AppendClass(HtmlHelper.ValidationInputCssClassName);
                }
            }
            //tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(fullName, metadata));

            return new HtmlString($"<select {htmlAttributes.FlattenToString()}>{optionsString}</select>");
        }

        internal static string ListItemToOption(SelectListStandardItem item)
        {
            var baseAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes);
            if (item.Value != null)
            {
                baseAttrs.AddUpdateHtmlAttribute("value", item.Value);
            }
            if (item.Selected)
            {
                baseAttrs.AddUpdateHtmlAttribute("selected", "selected");
            }
            var str = $"<option {baseAttrs.FlattenToString()}>{HttpUtility.HtmlEncode(item.Text)}</option>";
            
            return str;
        }

    }
}