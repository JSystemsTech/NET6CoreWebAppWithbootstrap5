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

    public class AuthenticationController : ApiControllerBase
    {
        private UserManager _UserManager => Services.GetRequiredService<UserManager>();
        public AuthenticationController(IServiceProvider services) : base(services) { }
        
#if OperationId
        [HttpPost("Authenticate", Name = nameof(Authenticate))]
#else
        [HttpPost("Authenticate")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates API Authentication JWT Token", typeof(APIResponse<APIAccessToken>))]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult Authenticate(string appId)
        {
            try
            {
                var response = DomainFacade.GetApplicationInfo(appId, out ApplicationInfo? applicationInfo);
                if (response.HasError)
                {
                    return ErrorResponse(response, APIAccessToken.Default);
                }
                if (applicationInfo is ApplicationInfo)
                {
                    return SuccessResponse(_UserManager.GetAccessToken(appId));
                }
                return ErrorResponse($"Unauthorized App '{appId}'", APIAccessToken.Default);
            }
            catch(Exception e)
            {
                return ErrorResponse(e, APIAccessToken.Default);
            }
        }
    }
}
