using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace NET6CoreWebAppWithbootstrap5.Attributes
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        private bool IsAjaxRequest(RouteContext routeContext)
        {
            var request = routeContext.HttpContext.Request;
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        => IsAjaxRequest(routeContext);
    }
}
