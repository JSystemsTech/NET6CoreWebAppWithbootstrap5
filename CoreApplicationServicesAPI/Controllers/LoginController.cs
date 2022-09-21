using CoreApplicationServicesAPI.DomainLayer;
using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace CoreApplicationServicesAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly UserManager _UserManager;
        public LoginController(UserManager userManager)
        {
            _UserManager = userManager;
        }
        
#if OperationId
        [HttpPost("Authenticate", Name = nameof(Authenticate))]
#else
        [HttpPost("Authenticate")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates API Authentication JWT Token", typeof(CoreApplicationServicesAPIResponse<CoreApplicationServicesAPIAccessToken>))]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<CoreApplicationServicesAPIAccessToken>> Authenticate(string appId)
        {
            try
            {
                var response = DomainFacade.GetApplicationInfo(appId, out ApplicationInfo? applicationInfo);
                if (response.HasError)
                {
                    return Ok(CoreApplicationServicesAPIResponse<CoreApplicationServicesAPIAccessToken>.Error($"{response.ErrorMessage} - {response.ErrorDetails}", CoreApplicationServicesAPIAccessToken.Default));
                }
                if (applicationInfo is ApplicationInfo)
                {
                    return Ok(CoreApplicationServicesAPIResponse <CoreApplicationServicesAPIAccessToken>.Create(_UserManager.GetAccessToken(appId)));
                }
                return Ok(CoreApplicationServicesAPIResponse<CoreApplicationServicesAPIAccessToken>.Error($"Unauthorized App '{appId}'", CoreApplicationServicesAPIAccessToken.Default));
            }
            catch(Exception e)
            {
                return Ok(CoreApplicationServicesAPIResponse<CoreApplicationServicesAPIAccessToken>.Error(e.Message, CoreApplicationServicesAPIAccessToken.Default));
            }
        }
    }
}
