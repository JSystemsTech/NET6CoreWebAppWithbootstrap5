using CoreApplicationServicesAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NET6CoreWebAppWithbootstrap5.Attributes;
using NET6CoreWebAppWithbootstrap5.ExampleData;
using NET6CoreWebAppWithbootstrap5.Extensions;
using NET6CoreWebAppWithbootstrap5.Extensions.Aspose.Cells;
using NET6CoreWebAppWithbootstrap5.Models;
using NET6CoreWebAppWithbootstrap5.Models.DataTable;
using NET6CoreWebAppWithbootstrap5.Models.Helpers;
using NET6CoreWebAppWithbootstrap5.Models.Helpers.DataTable;
using NET6CoreWebAppWithbootstrap5.Models.Home;
using NET6CoreWebAppWithbootstrap5.Services;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Dynamic;

namespace NET6CoreWebAppWithbootstrap5.Controllers
{
    [AllowAnonymous]
    public abstract class ControllerBase: Controller
    {
        private static string ThemeCookieName = "__ThemeInfo";
        protected IApplicationServices ApplicationServices { get; private set; }
        protected INotificationFactory Notification => ApplicationServices.Notification;
        protected ControllerBase(IApplicationServices applicationServices)
        {
            ApplicationServices = applicationServices;
            DataTableOptionsMap = new ConcurrentDictionary<string, (string url, object options)>();
        }
        [NonAction]
        public override JsonResult Json(object? data)
        {
            return new JsonResult(data.Merge(ResolveJsonGlobalValues()));
        }
        public JsonResult Json(object? data, Notification notification)
            => Json(data.Merge(ResolveJsonGlobalValues(notification)));

        public JsonResult InvalidModelStateJson(string title)
            => Json(new
            {
                modelStateErrors = ModelState.Where(kv => kv.Value is ModelStateEntry val && val.Errors.Count() > 0).ToDictionary(kv => kv.Key, kv => string.Join(", ", kv.Value is ModelStateEntry mse ? mse.Errors.Select(e => GetErrorMessage(e)) : ""))
            }, Notification.Warning(title, $"Model validation did not pass. Please check the form for more infomation"));

        protected virtual object ResolveJsonGlobalValues(Notification? notification = null) => new
        {
            notification = notification
        };
        private string GetErrorMessage(ModelError e)
        => string.IsNullOrWhiteSpace(e.ErrorMessage) ? (e.Exception is Exception ex ? ex.Message: "" ): e.ErrorMessage;
        protected async Task<string> RenderViewToStringAsync(string viewName, object? model)
            => await ApplicationServices.ViewRenderService.RenderToStringAsync(viewName, model);
        
        private ConcurrentDictionary<string, (string url, object options)> DataTableOptionsMap { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.AppUserData = ApplicationServices.ApplicationPricipalIdentityManager.GetAppUserData(HttpContext);


            ResolveDataTableOptions();
            dynamic DataTableOptions = new ExpandoObject();
            var dtOpts = DataTableOptions as IDictionary<string, object>;
            if(dtOpts != null)
            {
                foreach (var kv in DataTableOptionsMap)
                {
                    dtOpts.Add(kv.Key, kv.Key);
                }
            }
            
            ViewBag.DataTableOptions = DataTableOptions;
            ResolveTheme(context);

        }
        protected void AddError(Exception e)
        {
            ViewBag.Errors = ViewBag.Errors ?? new Exception[0];
            if (ViewBag.Errors is IEnumerable<Exception> errors)
            {
                ViewBag.Errors = errors.Append(e);
            }
        }
        protected async Task<string> GetDataTableColumnTemplateAsync<TModel>(string template, TModel model)
            => await RenderViewToStringAsync($"~/Views/Datatable/ColumnTemplates/{template}.cshtml", model);
        protected async Task<string> GetDataTableColumnActionButtonsAsync(params ActionButtonVM[] buttons)
            => await GetDataTableColumnTemplateAsync("ActionButtons", buttons);
        protected async Task<string> GetDataTableColumnImageAsync(string name, Guid? fileId, bool useDefault = false)
            => await GetDataTableColumnTemplateAsync("Image", (name, fileId, useDefault));

        [AllowAnonymous]
        public virtual ActionResult Unauthorized(string Message)
        {
            ViewBag.UnauthorizedMessage = Message;
            return View("Unauthorized");
        }
        protected virtual RedirectToActionResult RedirectToUnauthorized(string Message)
        => RedirectToAction("Unauthorized", Message);
        protected virtual PartialViewResult UnauthorizedPartialView(string Message)
        {
            ViewBag.UnauthorizedMessage = Message;
            return PartialView("Unauthorized", Message);
        }
        public virtual JsonResult GetDataTableOptions(string key)
        {
            if (DataTableOptionsMap.TryGetValue(key, out (string url, object options) config))
            {
                return new JsonResult(config.options);// Json(config.options);
            }
            return Json(new { error = true },Notification.Warning("Invalid Datatable Options Key",$"No Datatable options found for key '{key}'"));
        }
        //public ActionResult ShowImage(Guid id)
        //{
        //    var response = AppDomainFacade.GetFile(id, out AppFileRecord file);
        //    if (response.HasError)
        //    {
        //        return new EmptyResult();
        //    }
        //    return File(file.Data, file.MIMEType, file.FileName);
        //}
        protected virtual void ResolveDataTableOptions() { }
        protected void RegisterDataTableOptions<T>(string key, DataTableOptions<T> options)
        where T : class
        {
            if(options.Ajax != null && options.Ajax.Url != null)
            {
                DataTableOptionsMap.TryAdd(key, (options.Ajax.Url, options));
            }
            
        }
        protected DataTableOptions<T> GetDataTableOptions<T>(string key)
         where T : class
        {
            if (DataTableOptionsMap.TryGetValue(key, out (string url, object options) config) && config.options is DataTableOptions<T> dtOptions)
            {
                return dtOptions;
            }
            return DataTableOptions<T>.Default;
        }

        protected DataTableOptions<T> GetDataTableOptions<T>()
         where T : class
        {
            if (DataTableOptionsMap.Values.FirstOrDefault(m => m.url == Request.Path.Value).options is DataTableOptions<T> dtOptions)
            {
                return dtOptions;
            }
            return DataTableOptions<T>.Default;
        }

        protected ActionResult DataTableResult<T>(
            IEnumerable<T> data,
            DataTableRequest request)
            where T : class
        => DataTableResult(GetDataTableOptions<T>(), data, request);
        protected ActionResult DataTableResult<T>(
            string key,
            IEnumerable<T> data,
            DataTableRequest request)
            where T : class
        => DataTableResult(GetDataTableOptions<T>(key), data, request);

        protected ActionResult DataTableResult<T>(
            DataTableOptions<T> options,
            IEnumerable<T> data,
            DataTableRequest request)
            where T : class
        {
            var response = data.ProcessDataTable(request, options);
            if (request.Download)
            {
                return File(
                    WorkbookExtensions.ImportToWorkbook(response.ToSystemDataTable()).ExportToXlxs(),
                    MSOfficeMime.Xlxs,
                    string.IsNullOrWhiteSpace(request.FileName) ? "DataTableDownload" : request.FileName
                    );
            }
            return Json(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeTheme()
        {
            return Redirect(Request.Headers["Referer"].ToString());
        }
        protected virtual void ResolveTheme(ActionExecutingContext context)
        {
            string theme = "Base";
            string mode = "light";
            if (HttpContext.Request.HasFormContentType && HttpContext.Request.Form.TryGetValue("__theme", out StringValues requestTheme)){
                theme = requestTheme.ToString();
            }
            else if(HttpContext.Request.Cookies.TryGetValue(ThemeCookieName, out string? value) && value is string cachedTheme){
                var themeConfig = cachedTheme.Split(':');
                theme = themeConfig[0];
                mode = themeConfig[1];
            }
            if (HttpContext.Request.HasFormContentType && HttpContext.Request.Form.TryGetValue("__themeMode", out StringValues requestThemeMode))
            {
                mode = requestThemeMode.ToString();
            }
            ViewBag.ThemePath = ApplicationServices.WebThemeService.GetTheme(theme, mode == "dark");
            ViewBag.Theme = theme;
            ViewBag.ThemeIsDarkMode = mode == "dark";
            ViewBag.ThemeSelectList = ApplicationServices.WebThemeService.GetThemeList(theme);
            context.HttpContext.Response.Cookies.Append(
                ThemeCookieName,
                $"{theme}:{mode}",
                new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                }
            );
        }
        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult Modal(Modal vm)
        {
            return PartialView("Modal", vm);
        }
        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult Offcanvas(Offcanvas vm)
        {
            return PartialView("Offcanvas", vm);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public virtual async Task<IActionResult> RedirectToExternalApplication(string appCode)
        {
            // Create RedirectResult and add URLHelper 
            var webAppInfo = (await ApplicationServices.CoreApplicationServicesAPIClient.GetWebApplicationInfoAsync(appCode)).Value.FirstOrDefault();
            if (webAppInfo != null)
            {
                return Redirect(webAppInfo.DefaultRedirectUrl);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> ApplicationLogo()
        {
            ApplicationInfo appInfo = (await ApplicationServices.CoreApplicationServicesAPIClient.GetApplicationInfoAsync(ApplicationConfiguration.ApplicationSettings.AppId)).Value;
            if (appInfo.HasError || appInfo.LogoFileData.Count() == 0 || string.IsNullOrWhiteSpace(appInfo.LogoFileContentType) || string.IsNullOrWhiteSpace(appInfo.LogoFileName))
            {
                return new EmptyResult();
            }
            return File(appInfo.LogoFileData, appInfo.LogoFileContentType, appInfo.LogoFileName);
        }
    }

    [Authorize]
    public abstract class AuthenticatedControllerBase : ControllerBase
    {
        protected AuthenticatedControllerBase(IApplicationServices applicationServices) : base(applicationServices) { }

        public override async Task<IActionResult> RedirectToExternalApplication(string appCode)
        {
            // Create RedirectResult and add URLHelper 
            var response = await ApplicationServices.CoreApplicationServicesAPIClient.GetWebApplicationInfoAsync(appCode);
            if(!response.HasError && response.Value.FirstOrDefault() is ApplicationInfo webAppInfo)
            {
                var userData = ApplicationServices.ApplicationPricipalIdentityManager.GetAppUserData(HttpContext);
                if(userData != null)
                {
                    var ssoTokenResponse = await ApplicationServices.AuthenticationTokenServiceClient.CreateSSOTokenAsync(new AuthenticationTokenService.SSOTokenParameters()
                    {
                        Edipi = userData.EDIPI,
                        Email = userData.Email,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        MiddleInitial = userData.MiddleInitial
                    });
                    if(!ssoTokenResponse.HasError)
                    {
                        var ssoToken = ssoTokenResponse.Value;
                        var result = new RedirectResult(webAppInfo.DefaultRedirectUrl, true);
                        HttpContext.Response.Headers.Add(webAppInfo.SsO_Header, new StringValues(ssoToken.Value));
                        return result;
                    }
                }
                return Redirect(webAppInfo.DefaultRedirectUrl);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IApplicationServices applicationServices) :base(applicationServices)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Bootstrap()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult DataTableExample()
        {
            return View(new SampleDataTableModelVM());
        }
        public IActionResult SampleModal()
        {
            return PartialView();
        }

        public async Task<IActionResult> ApplicationSettings()
        {
            ApplicationInfo appInfo = (await ApplicationServices.CoreApplicationServicesAPIClient.GetApplicationInfoAsync(ApplicationConfiguration.ApplicationSettings.AppId)).Value;
            
            var vm = new ApplicationSettingsVM(appInfo, ApplicationServices.WebThemeService.GetThemeList(appInfo.Theme));
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplicationSettings(ApplicationSettingsVM vm)
        {
            vm.ThemeSelectList = ApplicationServices.WebThemeService.GetThemeList(vm.Theme);
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var parameters = new ApplicationInfoParameters()
            {
                Title = vm.Title,
                Description = vm.Description,
                Theme = vm.Theme,
                DefaultRedirectUrl = vm.DefaultRedirectUrl,
                SsoUrl = vm.SsoUrl,
                SsO_Header = vm.SsO_Header,
                ApplicationAPIUrl = vm.ApplicationAPIUrl,
                RequireRegistration = vm.RequireRegistration
            };
            if (vm.ApplicationLogo is IFormFile file)
            {
                string fileName = Path.GetFileName(vm.ApplicationLogo.FileName);
                string contentType = vm.ApplicationLogo.ContentType;
                using var fileStream = file.OpenReadStream();
                byte[] bytes = new byte[file.Length];
                fileStream.Read(bytes, 0, (int)file.Length);

                parameters.LogoFileName = fileName;
                parameters.LogoFileContentType = contentType;
                parameters.LogoFileData = bytes;
            }


            var response = await ApplicationServices.CoreApplicationServicesAPIClient.UpdateApplicationInfoAsync(parameters);

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public IActionResult DataTableExampleData(DataTableRequest request)
        {
            return DataTableResult(ExampleDataModel.DataSet, request);
        }
        protected override void ResolveDataTableOptions()
        {
            RegisterDataTableOptions(
                "DataTableExampleDataOptions",
                DataTableOptions<ExampleDataModel>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                },
                ajax =>
                {
                    ajax.Url = Url.Action("DataTableExampleData");
                }, columns =>
                {
                    columns.Add(m => m.FirstName, c => { c.Data = "FirstName"; c.Name = "FirstName"; c.Title = "First Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.LastName, c => { c.Data = "LastName"; c.Name = "LastName"; c.Title = "Last Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Age, c => { c.Data = "Age"; c.Name = "Age"; c.Title = "Age"; c.Filter = new DataTableOptionsFilter(); });
                    //columns.Add(m => m.Guid, c => {
                    //    c.Data = "Actions";
                    //    c.Name = "Actions";
                    //    c.Title = "Actions";
                    //    c.Searchable = false;
                    //    c.Orderable = false;
                    //    c.Export = false;
                    //    c.Render = m => GetDataTableColumnActionButtons(
                    //        new ActionButtonVM() { Title = "Manage Roles", Action = "manage-roles", ButtonClass = "btn-primary", Description = $"Manage {m.Name}'s Roles", Icon = "fa-user" },
                    //        new ActionButtonVM() { Title = "Manage Groups", Action = "manage-groups", ButtonClass = "btn-secondary", Description = $"Manage {m.Name}'s Groups", Icon = "fa-users" }
                    //    );
                    //});
                    //columns.Add(m => Url.Action("ManageUserRoles", new { userGuid = m.Guid }), c => { c.DataOnly = true; c.Data = "ManageUserRolesUrl"; });
                }));
            
        }

    }
}