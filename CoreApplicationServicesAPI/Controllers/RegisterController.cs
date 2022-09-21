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
    [Authorize]
    public class RegisterController : ApiControllerBase
    {
        public RegisterController(IServiceProvider services)
            : base(services) { }

#if OperationId
        [HttpPost("UserIsRegistered", Name = nameof(UserIsRegistered))]
#else
        [HttpPost("UserIsRegistered")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Registers User for Application", typeof(CoreApplicationServicesAPIResponse<bool>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<bool>> UserIsRegistered(string appId, ApplicationUserRegistrationAPI.RegisterUserParameters parameters)
        {
            try
            {
                
                var response = DomainFacade.GetApplicationInfo(appId, out ApplicationInfo? applicationInfo);
                if (response.HasError)
                {
                    return ErrorResponse(response,false);
                }
                if (applicationInfo is ApplicationInfo)
                {
                    if (applicationInfo.ApplicationAPIConfig != null)
                    {
                        var apiResponse = _ApplicationUserRegistrationAPIClient.UserIsRegisteredAsync(appId, parameters, applicationInfo.ApplicationAPIConfig).GetAwaiter().GetResult();
                        if (apiResponse.HasError)
                        {
                            return ErrorResponse(apiResponse.ErrorMessage, false);
                        }
                        return SuccessResponse(apiResponse.Value);
                    }
                }
                return ErrorResponse("Missing ApplicationInfo object", false);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, false);
            }
        }

        #if OperationId
        [HttpPost("RegisterUser", Name = nameof(RegisterUser))]
        #else
        [HttpPost("RegisterUser")]
        #endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Updates Application API Configuration Record", typeof(CoreApplicationServicesAPIResponse<bool>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<bool>> RegisterUser(string appId, ApplicationUserRegistrationAPI.RegisterUserParameters parameters)
        {
            try
            {

                var response = DomainFacade.GetApplicationInfo(appId, out ApplicationInfo? applicationInfo);
                if (response.HasError)
                {
                    return ErrorResponse(response, false);
                }
                if (applicationInfo is ApplicationInfo)
                {
                    if (applicationInfo.ApplicationAPIConfig != null)
                    {
                        var apiResponse = _ApplicationUserRegistrationAPIClient.RegisterUserAsync(appId, parameters, applicationInfo.ApplicationAPIConfig).GetAwaiter().GetResult();
                        if (apiResponse.HasError)
                        {
                            return ErrorResponse(apiResponse.ErrorMessage, false);
                        }
                        return SuccessResponse(apiResponse.Value);
                    }
                }
                return ErrorResponse("Missing ApplicationInfo object", false);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, false);
            }
        }

        
    }
}
