using Aspose.Diagram;
using Markdig.Renderers.Html;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NET6CoreWebAppWithbootstrap5.Models.Helpers.DataTable;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html.Bootstrap
{
    public static class TagBuilderExtensions
    {
        internal static string GetAttributesString(this TagBuilder tagBuilder)
        => string.Join(" ", tagBuilder.Attributes.Select(kv => $"{kv.Key}=\"{kv.Value}\""));
        internal static string GetStartTag(this TagBuilder tagBuilder)
        => $"<{tagBuilder.TagName} {tagBuilder.GetAttributesString()}>";
        internal static string GetEndTag(this TagBuilder tagBuilder)
        => $"</{tagBuilder.TagName}>";
    }
    public static class NavItemExtensions
    {
        public static MvcTag BeginNavItem(this IHtmlHelper htmlHelper, bool active = false)
            => htmlHelper.BeginNavItem(new { }, active);
        public static MvcTag BeginNavItem(this IHtmlHelper htmlHelper, object htmlAttributes, bool active = false)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary();
            attrs.AppendClass("nav-item");
            if (active)
            {
                attrs.AppendClass("active");
            }
            htmlHelper.BeginLi(attrs);
            return new MvcTag(htmlHelper.ViewContext, EndLi);
        }
        private static void BeginLi(this IHtmlHelper htmlHelper, IDictionary<string, object?> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("li");
            tagBuilder.MergeAttributes(htmlAttributes);
            htmlHelper.ViewContext.Writer.Write(tagBuilder.GetStartTag());
        }
        private static string EndLi => new TagBuilder("li").GetEndTag();
    }
    public static class NavbarExtensions
    {
        private static object defaultNavLinkHtmlAttrs = (new { }).AddNavLinkClass();
        private static object AddNavLinkClass(this object htmlAttributes)
        {
            return htmlAttributes.ToHtmlAttributesDictionary().AppendClass("nav-link");
        }
        private static IHtmlContent CreateNavItem(
            this IHtmlContent content)
        {
            return new HtmlString($"<li class=\"nav-item\">{content.GetString()}</li>");
        }
        public static IHtmlContent NavItemActionLink(
            this IHtmlHelper helper,
            string linkText,
            string actionName)
        => helper.ActionLink(linkText, actionName, new { }, defaultNavLinkHtmlAttrs).CreateNavItem();



        public static IHtmlContent NavItemActionLink(
            this IHtmlHelper helper,
            string linkText,
            string actionName,
            object routeValues)
        => helper.ActionLink(linkText, actionName, routeValues, defaultNavLinkHtmlAttrs).CreateNavItem();


        public static IHtmlContent NavItemActionLink(
            this IHtmlHelper helper,
            string linkText,
            string actionName,
            object routeValues,
            object htmlAttributes)

        => helper.ActionLink(linkText, actionName, routeValues, htmlAttributes.AddNavLinkClass()).CreateNavItem();


        public static IHtmlContent NavItemActionLink(
            this IHtmlHelper helper,
            string linkText,
            string actionName,
        string controllerName)

        => helper.ActionLink(linkText, actionName, controllerName, new { }, defaultNavLinkHtmlAttrs).CreateNavItem();

        public static IHtmlContent NavItemActionLink(
            this IHtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues)

        => helper.ActionLink(linkText, actionName, controllerName, routeValues, defaultNavLinkHtmlAttrs).CreateNavItem();


        public static IHtmlContent NavItemActionLink(
            this IHtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes)

        => helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes.AddNavLinkClass()).CreateNavItem();


        public static IHtmlContent NavItemRouteLink(
        this IHtmlHelper htmlHelper,
        string linkText,
        object routeValues)
        => htmlHelper.RouteLink(linkText, routeValues, defaultNavLinkHtmlAttrs).CreateNavItem();


        public static IHtmlContent NavItemRouteLink(
            this IHtmlHelper htmlHelper,
            string linkText,
            string routeName)
        => htmlHelper.RouteLink(linkText, routeName, defaultNavLinkHtmlAttrs).CreateNavItem();


        public static IHtmlContent NavItemRouteLink(
            this IHtmlHelper htmlHelper,
            string linkText,
            string routeName,
            object routeValues)
        => htmlHelper.RouteLink(linkText, routeName, routeValues, defaultNavLinkHtmlAttrs).CreateNavItem();


        public static IHtmlContent NavItemRouteLink(
            this IHtmlHelper htmlHelper,
            string linkText,
            object routeValues,
            object htmlAttributes)
        => htmlHelper.RouteLink(linkText, routeValues, htmlAttributes.AddNavLinkClass()).CreateNavItem();
        public static IHtmlContent NavItemRouteLink(
            this IHtmlHelper htmlHelper,
            string linkText,
            string routeName,
            object routeValues,
            object htmlAttributes)
        => htmlHelper.RouteLink(linkText, routeName, routeValues, htmlAttributes.AddNavLinkClass()).CreateNavItem();
    }
    public static class GroupExtensions
    {
        public static MvcTag BeginFormGroup(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginFormGroup(new { });
        public static MvcTag BeginFormGroup(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginGroup(htmlAttributes, "form-group");

        public static MvcTag BeginButtonGroup(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginButtonGroup(new { });
        public static MvcTag BeginButtonGroup(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginGroup(htmlAttributes, "btn-group");

        public static MvcTag BeginListGroup(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginListGroup(new { });
        public static MvcTag BeginListGroup(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginGroup(htmlAttributes, "list-group", "ul");
        public static MvcTag BeginListGroupDiv(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginListGroupDiv(new { });
        public static MvcTag BeginListGroupDiv(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginGroup(htmlAttributes, "list-group");



        public static MvcTag BeginInputGroup(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginInputGroup(new { });
        public static MvcTag BeginInputGroup(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginGroup(htmlAttributes, "input-group");

        public static MvcTag BeginInputGroupText(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginInputGroupText(new { });
        public static MvcTag BeginInputGroupText(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginGroup(htmlAttributes, "input-group-text");

        public static IHtmlContent InputGroupTextLabelFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            => htmlHelper.InputGroupTextLabelFor(expression, new { });
        public static IHtmlContent InputGroupTextLabelFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            => htmlHelper.InputGroupTextLabelFor(expression, htmlAttributes.ToHtmlAttributesDictionary());
        internal static IHtmlContent InputGroupTextLabelFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object?> htmlAttributes)
        {
            htmlAttributes.AppendClass("input-group-text");
            return htmlHelper.LabelFor(expression, htmlAttributes);
        }


        internal static MvcTag BeginGroup(this IHtmlHelper htmlHelper, object htmlAttributes, string groupClass, string tag = "div")
        => htmlHelper.BeginGroup(htmlAttributes.ToHtmlAttributesDictionary(), groupClass, tag);
        internal static MvcTag BeginGroup(this IHtmlHelper htmlHelper, IDictionary<string, object?> htmlAttributes, string groupClass, string tag = "div")
        {
            htmlAttributes.AppendClass(groupClass);
            TagBuilder tagBuilder = new TagBuilder(tag);
            tagBuilder.MergeAttributes(htmlAttributes);
            htmlHelper.ViewContext.Writer.Write(tagBuilder.GetStartTag());
            return new MvcTag(htmlHelper.ViewContext, tagBuilder.GetEndTag());
        }
    }
    public static class ListGroupItemExtensions
    {
        public static IHtmlContent ListGroupItem(this IHtmlHelper htmlHelper, string text, bool active = false, bool disabled = false)
        => htmlHelper.ListGroupItem(text, new { }, active, disabled);
        public static IHtmlContent ListGroupItem(this IHtmlHelper htmlHelper, string text, object htmlAttributes, bool active = false, bool disabled = false)
        => htmlHelper.ListGroupItem(text, htmlAttributes.ToHtmlAttributesDictionary(), active, disabled);
        private static IHtmlContent ListGroupItem(this IHtmlHelper htmlHelper, string text, IDictionary<string, object?> htmlAttributes, bool active = false, bool disabled = false)
        {
            var attrs = htmlAttributes.AppendClass("list-group-item");
            if (active)
            {
                attrs.AppendHtmlAttribute("aria-current", "true").AppendClass("active");
            }
            else if (disabled)
            {
                attrs.AppendHtmlAttribute("aria-disabled", "true").AppendClass("disabled");
            }
            return new HtmlString($"<li {attrs.ToHtmlAttributeString()}>{text}</li>");
        }
        public static IHtmlContent ListGroupItemLink(this IHtmlHelper htmlHelper, string linkText, string href, bool active = false, bool disabled = false, bool openNewWindow = false)
        => htmlHelper.ListGroupItemLink(linkText, href, new { }, active, disabled, openNewWindow);
        public static IHtmlContent ListGroupItemLink(this IHtmlHelper htmlHelper, string linkText, string href, object htmlAttributes, bool active = false, bool disabled = false, bool openNewWindow = false)
        => htmlHelper.ListGroupItemLink(linkText, href, htmlAttributes.ToHtmlAttributesDictionary(), active, disabled, openNewWindow);
        private static IHtmlContent ListGroupItemLink(this IHtmlHelper htmlHelper, string linkText, string href, IDictionary<string, object?> htmlAttributes, bool active = false, bool disabled = false, bool openNewWindow = false)
        {
            var attrs = htmlAttributes.AppendHtmlAttribute("href", href).AppendClass("list-group-item").AppendClass("list-group-item-action");
            if (openNewWindow)
            {
                attrs.AppendHtmlAttribute("target", "_blank").AppendHtmlAttribute("rel", "noopener noreferrer");
            }
            if (active)
            {
                attrs.AppendHtmlAttribute("aria-current", "true").AppendClass("active");
            }
            else if (disabled)
            {
                attrs.AppendHtmlAttribute("aria-disabled", "true").AppendHtmlAttribute("tab-index", "-1").AppendClass("disabled");
            }
            return new HtmlString($"<a {attrs.ToHtmlAttributeString()}>{linkText}</a>");
        }

        public static IHtmlContent ListGroupItemButton(this IHtmlHelper htmlHelper, string text, bool active = false, bool disabled = false)
        => htmlHelper.ListGroupItemButton(text, new { }, active, disabled);
        public static IHtmlContent ListGroupItemButton(this IHtmlHelper htmlHelper, string text, object htmlAttributes, bool active = false, bool disabled = false)
        => htmlHelper.ListGroupItemButton(text, "button", htmlAttributes.ToHtmlAttributesDictionary(), active, disabled);
        public static IHtmlContent ListGroupItemButton(this IHtmlHelper htmlHelper, string text, string type, bool active = false, bool disabled = false)
        => htmlHelper.ListGroupItemButton(text, type, new { }, active, disabled);
        public static IHtmlContent ListGroupItemButton(this IHtmlHelper htmlHelper, string text, string type, object htmlAttributes, bool active = false, bool disabled = false)
        => htmlHelper.ListGroupItemButton(text, type, htmlAttributes.ToHtmlAttributesDictionary(), active, disabled);
        private static IHtmlContent ListGroupItemButton(this IHtmlHelper htmlHelper, string text, string type, IDictionary<string, object?> htmlAttributes, bool active = false, bool disabled = false)
        {
            var attrs = htmlAttributes.AppendHtmlAttribute("type", type).AppendClass("list-group-item").AppendClass("list-group-item-action");

            if (active)
            {
                attrs.AppendHtmlAttribute("aria-current", "true").AppendClass("active");
            }
            else if (disabled)
            {
                attrs.AppendHtmlAttribute("disabled", "true");
            }
            return new HtmlString($"<button {attrs.ToHtmlAttributeString()}>{text}</button>");
        }


    }
    public static class TabExtensions
    {
        public static MvcTag BeginTabListGroup(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginTabListGroup(new { });
        public static MvcTag BeginTabListGroup(this IHtmlHelper htmlHelper, object htmlAttributes)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendHtmlAttribute("role", "tablist");
            return htmlHelper.BeginGroup(attrs, "list-group");
        }

        public static IHtmlContent TabListGroupItemLink(this IHtmlHelper htmlHelper, string linkText, string target, bool active = false, bool disabled = false)
        => htmlHelper.TabListGroupItemLink(linkText, target, new { }, active, disabled);
        public static IHtmlContent TabListGroupItemLink(this IHtmlHelper htmlHelper, string linkText, string target, object htmlAttributes, bool active = false, bool disabled = false)
        => htmlHelper.TabListGroupItemLink(linkText, target, htmlAttributes.ToHtmlAttributesDictionary(), active, disabled);
        private static IHtmlContent TabListGroupItemLink(this IHtmlHelper htmlHelper, string linkText, string target, IDictionary<string, object?> htmlAttributes, bool active = false, bool disabled = false)
        {
            var attrs = htmlAttributes.AppendHtmlAttribute("href", $"#{target}").AppendHtmlAttribute("aria-controls", target).AppendHtmlAttribute("data-toggle", "list").AppendHtmlAttribute("role", "tab").AppendClass("list-group-item").AppendClass("list-group-item-action");

            if (active)
            {
                attrs.AppendHtmlAttribute("aria-current", "true").AppendClass("active");
            }
            else if (disabled)
            {
                attrs.AppendHtmlAttribute("aria-disabled", "true").AppendHtmlAttribute("tab-index", "-1").AppendClass("disabled");
            }
            return new HtmlString($"<a {attrs.ToHtmlAttributeString()}>{linkText}</a>");
        }

        public static MvcTag BeginTabContent(this IHtmlHelper htmlHelper, string tag = "div")
        => htmlHelper.BeginTabContent(new { }, tag);
        public static MvcTag BeginTabContent(this IHtmlHelper htmlHelper, object htmlAttributes, string tag = "div")
        => htmlHelper.BeginGroup(htmlAttributes, "tab-content", tag);

        public static MvcTag BeginTabPane(this IHtmlHelper htmlHelper, string labelledby, bool active = false, string tag = "div")
            => htmlHelper.BeginTabPane(labelledby, new { }, active, tag);
        public static MvcTag BeginTabPane(this IHtmlHelper htmlHelper, string labelledby, object htmlAttributes, bool active = false, string tag = "div")
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendHtmlAttribute("role", "tabpanel").AppendHtmlAttribute("aria-labelledby", labelledby);

            if (active)
            {
                attrs.AppendClass("show").AppendClass("active");
            }
            return htmlHelper.BeginGroup(attrs, "tab-pane", tag);
        }

    }
    public static class ButtonExtensions
    {
        public static IHtmlContent Button(this IHtmlHelper htmlHelper, string text)
        => htmlHelper.Button(text, new { });
        public static IHtmlContent Button(this IHtmlHelper htmlHelper, string type, string text)
        => htmlHelper.Button(type, text, new { });
        public static IHtmlContent Button(this IHtmlHelper htmlHelper, string text, object htmlAttributes)
        => htmlHelper.Button("button", text, htmlAttributes);
        public static IHtmlContent Button(this IHtmlHelper htmlHelper, string type, string text, object htmlAttributes)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendHtmlAttribute("type", type).AppendClass("btn");
            return new HtmlString($"<button {attrs.ToHtmlAttributeString()}>{text}</button>");
        }
        private static IHtmlContent InputSubmitButton(this IHtmlHelper htmlHelper, string text, object htmlAttributes)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendHtmlAttribute("type", "submit").AppendHtmlAttribute("name", text).AppendHtmlAttribute("value", text).AppendClass("btn");
            return new HtmlString($"<input {attrs.ToHtmlAttributeString()}></input>");
        }

        public static IHtmlContent SubmitButton(this IHtmlHelper htmlHelper, string text)
        => htmlHelper.SubmitButton(text, new { });
        public static IHtmlContent SubmitButton(this IHtmlHelper htmlHelper, string text, object htmlAttributes)
        => htmlHelper.InputSubmitButton(text, htmlAttributes);

        public static IHtmlContent SubmitButtonWithIcon(this IHtmlHelper htmlHelper, string text, bool iconRight = false)
        => htmlHelper.SubmitButtonWithIcon(text, new { }, iconRight);
        public static IHtmlContent SubmitButtonWithIcon(this IHtmlHelper htmlHelper, string text, string iconClass, bool iconRight = false)
        => htmlHelper.SubmitButtonWithIcon(text, iconClass, new { }, iconRight);
        public static IHtmlContent SubmitButtonWithIcon(this IHtmlHelper htmlHelper, string text, object htmlAttributes, bool iconRight = false)
        => htmlHelper.SubmitButtonWithIcon(text, "fas fa-sign-in-alt", htmlAttributes, iconRight);
        public static IHtmlContent SubmitButtonWithIcon(this IHtmlHelper htmlHelper, string text, string iconClass, object htmlAttributes, bool iconRight = false)
        => htmlHelper.ButtonWithIcon("submit", text, iconClass, htmlAttributes, iconRight);


        public static IHtmlContent ButtonWithIcon(this IHtmlHelper htmlHelper, string text, string iconClass, bool iconRight = false)
        => htmlHelper.ButtonWithIcon(text, iconClass, new { }, iconRight);
        public static IHtmlContent ButtonWithIcon(this IHtmlHelper htmlHelper, string type, string text, string iconClass, bool iconRight = false)
        => htmlHelper.ButtonWithIcon(type, text, iconClass, new { }, iconRight);
        public static IHtmlContent ButtonWithIcon(this IHtmlHelper htmlHelper, string text, string iconClass, object htmlAttributes, bool iconRight = false)
        => htmlHelper.ButtonWithIcon("button", text, iconClass, htmlAttributes, iconRight);
        private const string FontFamilyBase = "fas";
        public static IHtmlContent ButtonWithIcon(this IHtmlHelper htmlHelper, string type, string text, string iconClass, object htmlAttributes, bool iconRight = false)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendHtmlAttribute("type", type).AppendClass("btn");
            var fontFamily = !iconClass.HasClass("far") && !iconClass.HasClass("fas") && !iconClass.HasClass("fab") ? FontFamilyBase : "";
            
            string iconEl = $"<i class=\"{fontFamily} {iconClass}\" aria-hidden=\"true\"></i>";
            string textEl = $"<span class=\"{(iconRight ? "me-1" : "ms-1")}\" btn-text=\"true\">{text}</span>";
            string buttonContent = iconRight ? $"{textEl}{iconEl}" : $"{iconEl}{textEl}";
            return new HtmlString($"<button {attrs.ToHtmlAttributeString()}>{buttonContent}</button>");
        }

        public static MvcTag BeginButton(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginButton(new { });
        public static MvcTag BeginButton(this IHtmlHelper htmlHelper, string type)
            => htmlHelper.BeginButton(type, new { });
        public static MvcTag BeginButton(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginButton("button", htmlAttributes);
        public static MvcTag BeginButton(this IHtmlHelper htmlHelper, string type, object htmlAttributes)
        {
            var attrs = htmlAttributes.ToHtmlAttributesDictionary().AppendHtmlAttribute("type", type).AppendClass("btn");

            TagBuilder tagBuilder = new TagBuilder("button");
            tagBuilder.MergeAttributes(attrs);
            htmlHelper.ViewContext.Writer.Write(tagBuilder.GetStartTag());
            return new MvcTag(htmlHelper.ViewContext, tagBuilder.GetEndTag());
        }

        public static MvcTag BeginSubmitButton(this IHtmlHelper htmlHelper)
            => htmlHelper.BeginSubmitButton(new { });
        public static MvcTag BeginSubmitButton(this IHtmlHelper htmlHelper, object htmlAttributes)
            => htmlHelper.BeginButton("submit", htmlAttributes);
    }
    public static class FontAwesomeExtensions
    {
        private const string FontFamilyBase = "fas";
        public static MvcTag BeginFaStack(this IHtmlHelper htmlHelper, object htmlAttributes, string tag = "span")
            => htmlHelper.BeginGroup(htmlAttributes, "fa-stack", tag);
        public static IHtmlContent FaIcon(this IHtmlHelper htmlHelper, string icon, string fontFamily = FontFamilyBase)
            => htmlHelper.FaIcon(icon, new { }, fontFamily);
        public static IHtmlContent FaIcon(this IHtmlHelper htmlHelper, string icon, object htmlAttributes, string fontFamily = FontFamilyBase)
        => htmlHelper.FaIcon(icon, htmlAttributes.ToHtmlAttributesDictionary(), fontFamily);
        private static IHtmlContent FaIcon(this IHtmlHelper htmlHelper, string icon, IDictionary<string, object?> htmlAttributes, string fontFamily = FontFamilyBase)
        {
            var attrs = htmlAttributes.AppendHtmlAttribute("aria-hidden", "true").AppendClass(icon);
            if (!attrs.HasClass("far") && !attrs.HasClass("fas") && !attrs.HasClass("fab"))
            {
                attrs = attrs.AppendClass(fontFamily);
            }
            return new HtmlString($"<i {attrs.ToHtmlAttributeString()}></i>");
        }

        public static IHtmlContent FaStackIcon2x(this IHtmlHelper htmlHelper, string fontFamily = FontFamilyBase)
            => htmlHelper.FaStackIcon2x(new { }, fontFamily);
        public static IHtmlContent FaStackIcon2x(this IHtmlHelper htmlHelper, object htmlAttributes, string fontFamily = FontFamilyBase)
        => htmlHelper.FaStackIcon(htmlAttributes.ToHtmlAttributesDictionary(), fontFamily, "fa-stack-2x");
        public static IHtmlContent FaStackIcon(this IHtmlHelper htmlHelper, string fontFamily = FontFamilyBase)
            => htmlHelper.FaStackIcon(new { }, fontFamily);
        public static IHtmlContent FaStackIcon(this IHtmlHelper htmlHelper, object htmlAttributes, string fontFamily = FontFamilyBase)
        => htmlHelper.FaStackIcon(htmlAttributes.ToHtmlAttributesDictionary(), fontFamily);
        private static IHtmlContent FaStackIcon(this IHtmlHelper htmlHelper, IDictionary<string, object?> htmlAttributes, string stackClass = "fa-stack-1x", string fontFamily = FontFamilyBase)
        {
            var attrs = htmlAttributes.AppendHtmlAttribute("aria-hidden", "true").AppendClass(fontFamily).AppendClass(stackClass);
            return new HtmlString($"<i {attrs.ToHtmlAttributeString()}></i>");
        }

        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            string iconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(new { }, new { @class = iconBottom }, iconTop, fontFamily);
        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            object htmlAttributesIconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(new { }, htmlAttributesIconBottom, new { @class = iconTop }, fontFamily);
        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            string iconBottom,
            object htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(new { }, new { @class = iconBottom }, htmlAttributesIconTop, fontFamily);

        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            string iconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(htmlAttributesStack, new { @class = iconBottom }, iconTop, fontFamily);
        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            object htmlAttributesIconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(htmlAttributesStack, htmlAttributesIconBottom, new { @class = iconTop }, fontFamily);
        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            string iconBottom,
            object htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(htmlAttributesStack, new { @class = iconBottom }, htmlAttributesIconTop, fontFamily);
        public static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            object htmlAttributesIconBottom,
            object htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack(htmlAttributesStack.ToHtmlAttributesDictionary(), htmlAttributesIconBottom.ToHtmlAttributesDictionary(), htmlAttributesIconTop.ToHtmlAttributesDictionary(), fontFamily);
        private static IHtmlContent FaStack(
            this IHtmlHelper htmlHelper,
            IDictionary<string, object?> htmlAttributesStack,
            IDictionary<string, object?> htmlAttributesIconBottom,
            IDictionary<string, object?> htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
        {
            var attrsStackAttrs = htmlAttributesStack.AppendClass("fa-stack").AppendClass("fa-stack-sm");
            var attrsIconBottomAttrs = htmlAttributesIconBottom.AppendHtmlAttribute("aria-hidden", "true").AppendClass("fa-stack-2x");
            if (!attrsIconBottomAttrs.HasClass("far") && !attrsIconBottomAttrs.HasClass("fas") && !attrsIconBottomAttrs.HasClass("fab"))
            {
                attrsIconBottomAttrs = attrsIconBottomAttrs.AppendClass(fontFamily);
            }
            var attrsIconTopAttrs = htmlAttributesIconTop.AppendHtmlAttribute("aria-hidden", "true").AppendClass("fa-stack-1x").AppendClass("stack-top").AppendClass("fa-inverse");
            if (!attrsIconTopAttrs.HasClass("far") && !attrsIconTopAttrs.HasClass("fas") && !attrsIconTopAttrs.HasClass("fab"))
            {
                attrsIconTopAttrs = attrsIconTopAttrs.AppendClass(fontFamily);
            }
            string iconBottomEl = $"<i {attrsIconBottomAttrs.ToHtmlAttributeString()}></i>";
            string iconTopEl = $"<i {attrsIconTopAttrs.ToHtmlAttributeString()}></i>";
            return new HtmlString($"<span {attrsStackAttrs.ToHtmlAttributeString()}>{iconBottomEl}{iconTopEl}</span>");
        }


        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            string iconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(new { }, new { @class = iconBottom }, iconTop, fontFamily);
        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            object htmlAttributesIconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(new { }, htmlAttributesIconBottom, new { @class = iconTop }, fontFamily);
        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            string iconBottom,
            object htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(new { }, new { @class = iconBottom }, htmlAttributesIconTop, fontFamily);

        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            string iconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(htmlAttributesStack, new { @class = iconBottom }, iconTop, fontFamily);
        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            object htmlAttributesIconBottom,
            string iconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(htmlAttributesStack, htmlAttributesIconBottom, new { @class = iconTop }, fontFamily);
        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            string iconBottom,
            object htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(htmlAttributesStack, new { @class = iconBottom }, htmlAttributesIconTop, fontFamily);
        public static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            object htmlAttributesStack,
            object htmlAttributesIconBottom,
            object htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
            => htmlHelper.FaStack2x(htmlAttributesStack.ToHtmlAttributesDictionary(), htmlAttributesIconBottom.ToHtmlAttributesDictionary(), htmlAttributesIconTop.ToHtmlAttributesDictionary(), fontFamily);
        private static IHtmlContent FaStack2x(
            this IHtmlHelper htmlHelper,
            IDictionary<string, object?> htmlAttributesStack,
            IDictionary<string, object?> htmlAttributesIconBottom,
            IDictionary<string, object?> htmlAttributesIconTop,
            string fontFamily = FontFamilyBase)
        {
            var attrsStackAttrs = htmlAttributesStack.AppendClass("fa-stack");
            var attrsIconBottomAttrs = htmlAttributesIconBottom.AppendHtmlAttribute("aria-hidden", "true").AppendClass("fa-stack-2x");
            if (!attrsIconBottomAttrs.HasClass("far") && !attrsIconBottomAttrs.HasClass("fas") && !attrsIconBottomAttrs.HasClass("fab"))
            {
                attrsIconBottomAttrs = attrsIconBottomAttrs.AppendClass(fontFamily);
            }
            var attrsIconTopAttrs = htmlAttributesIconTop.AppendHtmlAttribute("aria-hidden", "true").AppendClass("fa-stack-1x").AppendClass("fa-inverse");
            if (!attrsIconTopAttrs.HasClass("far") && !attrsIconTopAttrs.HasClass("fas") && !attrsIconTopAttrs.HasClass("fab"))
            {
                attrsIconTopAttrs = attrsIconTopAttrs.AppendClass(fontFamily);
            }

            string iconBottomEl = $"<i {attrsIconBottomAttrs.ToHtmlAttributeString()}></i>";
            string iconTopEl = $"<i {attrsIconTopAttrs.ToHtmlAttributeString()}></i>";
            return new HtmlString($"<span {attrsStackAttrs.ToHtmlAttributeString()}>{iconBottomEl}{iconTopEl}</span>");
        }
    }
}