using CoreApplicationServicesAPI.DomainLayer;
using CoreApplicationServicesAPI.DomainLayer.Models.Data;
using CoreApplicationServicesAPI.DomainLayer.Models.Parameters;
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
    public class ApplicationInfoController : ApiControllerBase
    {
        public ApplicationInfoController(IServiceProvider services)
            : base(services) { }

#if OperationId
        [HttpGet("GetApplicationInfo", Name = nameof(GetApplicationInfo))]
#else
        [HttpGet("GetApplicationInfo")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Gets Application Info Record", typeof(CoreApplicationServicesAPIResponse<ApplicationInfo>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<ApplicationInfo>> GetApplicationInfo(string? appId = null)
        {
            try
            {
                var userAppId = User.Identity is ClaimsIdentity ci && ci.Claims.FirstOrDefault(c => c.Type == ClaimTypes.System) is Claim claim ? claim.Value : null;
                string? appIdToUse = appId ?? userAppId;
                if (appIdToUse == null)
                {
                    return ErrorResponse("Missing 'appId' in parameters or on header", ApplicationInfo.Default);
                }
                var response = DomainFacade.GetApplicationInfo(appIdToUse, out ApplicationInfo? applicationInfo);
                if (response.HasError)
                {
                    return ErrorResponse(response, ApplicationInfo.Default);
                }
                if (applicationInfo is ApplicationInfo)
                {
                    return SuccessResponse(applicationInfo);
                }
                return ErrorResponse("Missing ApplicationInfo object", ApplicationInfo.Default);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, ApplicationInfo.Default);
            }
        }
#if OperationId
        [HttpPost("UpdateApplicationInfo", Name = nameof(UpdateApplicationInfo))]
#else
            [HttpPost("UpdateApplicationInfo")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Updates Application Info Record", typeof(CoreApplicationServicesAPIResponse<ApplicationInfo>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<ApplicationInfo>> UpdateApplicationInfo(ApplicationInfoParameters applicationInfo)
        {
            try
            {
                string? appId = User.Identity is ClaimsIdentity ci && ci.Claims.FirstOrDefault(c => c.Type == ClaimTypes.System) is Claim claim ? claim.Value : null;

                if (appId == null)
                {
                    return ErrorResponse("Missing 'appId' on header", ApplicationInfo.Default);
                }
                var updateResponse = DomainFacade.UpdateApplicationInfo(appId, applicationInfo);
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


#if OperationId
        [HttpGet("GetWebApplicationInfo", Name = nameof(GetWebApplicationInfo))]
#else
        [HttpGet("GetWebApplicationInfo")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Gets Web Application Info Record", typeof(CoreApplicationServicesAPIResponse<IEnumerable<ApplicationInfo>>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<IEnumerable<ApplicationInfo>>> GetWebApplicationInfo(string? appCode = null)
        {
            IEnumerable<ApplicationInfo> defaultValue = Array.Empty<ApplicationInfo>();
            try
            {


                var response = DomainFacade.GetWebApplicationInfo(appCode);
                if (response.HasError)
                {
                    return ErrorResponse(response, defaultValue);
                }
                return SuccessListResponse(response);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, defaultValue);
            }
        }

#if OperationId
        [HttpGet("ApplicationExists", Name = nameof(ApplicationExists))]
#else
        [HttpGet("ApplicationExists")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Checks to see if Application Info Record exists", typeof(CoreApplicationServicesAPIResponse<bool>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<bool>> ApplicationExists(string appId)
        {
            try
            {
                var response = DomainFacade.GetApplicationInfo(appId, out ApplicationInfo? applicationInfo);
                if (response.HasError)
                {
                    return ErrorResponse(response, false);
                }
                return SuccessResponse(!response.HasError && applicationInfo is ApplicationInfo);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, false);
            }
        }

    }
}
