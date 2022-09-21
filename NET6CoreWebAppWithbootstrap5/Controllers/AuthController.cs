using Aspose.Slides.Export.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NET6CoreWebAppWithbootstrap5.Controllers;
using NET6CoreWebAppWithbootstrap5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace NET6CoreWebAppWithbootstrap5.Controllers
{

    public class AuthController : AuthenticatedControllerBase
    {
        public AuthController(IApplicationServices applicationServices) : base(applicationServices) { }
        
        public IActionResult ExampleAuthenticatedPage()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> TestUserLogin()
        {            
            await ApplicationServices.ApplicationPricipalIdentityManager.SignInAsync(HttpContext, "1234567890", "Test User", "Test", "User", "L", "test.user@user_email_address.com");
            return RedirectToAction("ExampleAuthenticatedPage");
        }
    }
}
