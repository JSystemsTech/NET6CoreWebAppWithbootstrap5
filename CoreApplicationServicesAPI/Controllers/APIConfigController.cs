using CoreApplicationServicesAPI.DomainLayer;
using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;

namespace CoreApplicationServicesAPI.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class APIConfigController : ApiControllerBase
    {
        public APIConfigController(IServiceProvider services)
            : base(services) { }
#if OperationId
        [HttpPost("UpdateApplicationAPIConfig", Name = nameof(UpdateApplicationAPIConfig))]
#else
        [HttpPost("UpdateApplicationAPIConfig")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Check to see if user is registered for Application", typeof(APIResponse<ApplicationInfo>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UpdateApplicationAPIConfig(ApplicationAPIConfig config)
        {
            try
            {
                string? appId = User.Identity is ClaimsIdentity ci && ci.Claims.FirstOrDefault(c => c.Type == ClaimTypes.System) is Claim claim ? claim.Value : null;

                if (appId == null)
                {
                    return ErrorResponse("Missing 'appId' on header", ApplicationInfo.Default);
                }
                var updateResponse = DomainFacade.UpdateApplicationInfoApplicationAPIConfig(appId, config);
                if (updateResponse.HasError)
                {
                    return ErrorResponse(updateResponse, ApplicationInfo.Default);
                }

                var response = DomainFacade.GetApplicationInfo(appId, out ApplicationInfo? newApplicationInfo);
                if (response.HasError)
                {
                    return ErrorResponse(response, ApplicationInfo.Default);
                }
                if (newApplicationInfo is ApplicationInfo)
                {
                    return SuccessResponse(newApplicationInfo);
                }
                return ErrorResponse("Missing ApplicationInfo object", ApplicationInfo.Default);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, ApplicationInfo.Default);
            }
        }

    }
}
