using AuthenticationTokenService;
using CoreApplicationServicesAPI;
using Markdig;
using Markdig.Extensions.AutoIdentifiers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NET6CoreWebAppWithbootstrap5.Models;
using NET6CoreWebAppWithbootstrap5.Services;
using NET6CoreWebAppWithbootstrap5.Services.Authentication;
using NET6CoreWebAppWithbootstrap5.Services.Configuration;
using Westwind.AspNetCore.Markdown;

//var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = Directory.GetCurrentDirectory()
});

//Configuration needed as static objects for use in extension helps
builder.Configuration.UseStaticApplicationConfiguration();



builder.Services.AddHttpClient<AuthenticationTokenServiceClient>().ConfigureHttpClient(options => {
    options.BaseAddress = new Uri(ApplicationConfiguration.OpenAPIUrlsSettings.AuthenticationTokenService);
});
builder.Services.AddHttpClient<CoreApplicationServicesAPIClient>().ConfigureHttpClient(options => {
    options.BaseAddress = new Uri(ApplicationConfiguration.OpenAPIUrlsSettings.CoreApplicationServicesAPI);
});

// Add services to the container.
builder.Services.AddMarkdown(config =>
{
    // optional Tag BlackList
    config.HtmlTagBlackList = "script|iframe|object|embed|form"; // default

    // Simplest: Use all default settings
    var folderConfig = config.AddMarkdownProcessingFolder("/docs/", "~/Views/__MarkdownPageTemplate.cshtml");

    // Customized Configuration: Set FolderConfiguration options
    //folderConfig = config.AddMarkdownProcessingFolder("/posts/", "~/Pages/__MarkdownPageTemplate.cshtml");

    // Optionally strip script/iframe/form/object/embed tags ++
    folderConfig.SanitizeHtml = true;  //  default

    // folderConfig.BasePath = "http://othersite.com";

    // Optional configuration settings
    folderConfig.ProcessExtensionlessUrls = true;  // default
    folderConfig.ProcessMdFiles = true; // default

    // Optional pre-processing - with filled model
    //folderConfig.PreProcess = (model, controller) =>
    //{
    //    // controller.ViewBag.Model = new MyCustomModel();
    //};


    // Create custom MarkdigPipeline 
    // using MarkDig; for extension methods
    config.ConfigureMarkdigPipeline = builder =>
    {
        builder.UseEmphasisExtras(Markdig.Extensions.EmphasisExtras.EmphasisExtraOptions.Default)
            .UsePipeTables()
            .UseGridTables()
            .UseAutoIdentifiers(AutoIdentifierOptions.GitHub) // Headers get id="name" 
            .UseAutoLinks() // URLs are parsed into anchors
            .UseAbbreviations()
            .UseYamlFrontMatter()
            .UseEmojiAndSmiley(true)
            .UseListExtras()
            .UseFigures()
            .UseTaskLists()
            .UseCustomContainers()
            .UseGenericAttributes()
            .DisableHtml();

        //.DisableHtml();   // don't render HTML - encode as text
    };
});





builder.Services.AddControllersWithViews()
    .AddApplicationPart(typeof(MarkdownPageProcessorMiddleware).Assembly);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
        options.EventsType = typeof(ApplicationCookieAuthenticationEvents);
    })
    .AddSSO();

builder.Services.AddScoped<ApplicationPricipalIdentityManager>();
builder.Services.AddScoped<ApplicationCookieAuthenticationEvents>(); //Middleware to validate auth token
builder.Services.AddScoped<IClaimsTransformation, RefreshClaimsTransformation>(); //Middleware to refresh claims from auth token

builder.Services.AddScoped<IWebThemeService, WebThemeService>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddScoped<INotificationFactory, NotificationFactory>();

//Summary of built services. add last before calling builder.Build();
builder.Services.AddScoped<IApplicationServices, ApplicationServices>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles(new DefaultFilesOptions()
{
    DefaultFileNames = new List<string> { "index.md", "index.html" }
});
app.UseMarkdown();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
