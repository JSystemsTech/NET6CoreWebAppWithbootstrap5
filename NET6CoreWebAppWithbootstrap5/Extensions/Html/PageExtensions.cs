using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Web;
using NET6CoreWebAppWithbootstrap5.Models.Helpers.DataTable;

namespace NET6CoreWebAppWithbootstrap5.Extensions.Html
{
    public static class PageExtensions
    {
        //public static Task<IHtmlContent> PageMenu(this IHtmlHelper htmlHelper, IEnumerable<Page> model)
        //=> htmlHelper.PartialAsync("PageMenu", model.GroupBy(m => string.IsNullOrWhiteSpace(m.Group) ? m.Guid.ToString() : m.Group));
        //public static Task<IHtmlContent> PageMenu(this IHtmlHelper htmlHelper, Guid pageGuid)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    IAdventureWorksDomainFacade adventureWorksDomainFacade = provider.GetService<IAdventureWorksDomainFacade>();
        //    return htmlHelper.PageMenu(adventureWorksDomainFacade.GetPageMenu(pageGuid));
        //}
        //public static Task<IHtmlContent> PageBreadcrumbMenu(this IHtmlHelper htmlHelper, Guid pageGuid)
        //{
        //    IServiceProvider provider = htmlHelper.ViewContext.HttpContext.RequestServices;
        //    IAdventureWorksDomainFacade adventureWorksDomainFacade = provider.GetService<IAdventureWorksDomainFacade>();
        //    List<Page> pages = new List<Page>();

        //    var current = adventureWorksDomainFacade.GetPage(pageGuid);
        //    pages.Add(current);
        //    Guid? parentGuid = current.ParentGuid;
        //    while (parentGuid is Guid parentPageGuid)
        //    {
        //        var next = adventureWorksDomainFacade.GetPage(parentPageGuid);
        //        pages.Add(next);
        //        parentGuid = next.ParentGuid;
        //    }
        //    pages.Reverse();
        //    return htmlHelper.PartialAsync("PageBreadcrumbMenu", pages);
        //}
        public static IHtmlContent DataTable(this IHtmlHelper htmlHelper, Action<DataTableViewVM> init)
        {
            DataTableViewVM vm = new DataTableViewVM();
            init(vm);
            return htmlHelper.DataTable(vm);
        }
        public static IHtmlContent DataTable(this IHtmlHelper htmlHelper, DataTableViewVM vm)
        => htmlHelper.Partial("Datatable", vm);
        public static IHtmlContent JSONSafeString(this IHtmlHelper htmlHelper, string json)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(json));
        }
        public static IHtmlContent JSONSafeString<T>(this IHtmlHelper htmlHelper, T model)
        {
            return new HtmlString(HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(model)));
        }
    }
}