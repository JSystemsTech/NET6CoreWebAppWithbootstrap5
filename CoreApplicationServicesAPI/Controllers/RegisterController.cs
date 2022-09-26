using ApplicationUserRegistrationAPI;
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
        private ApplicationUserRegistrationAPIClient _ApplicationUserRegistrationAPIClient => Services.GetRequiredService<ApplicationUserRegistrationAPIClient>();
        public RegisterController(IServiceProvider services)
            : base(services) { }

#if OperationId
        [HttpPost("UserIsRegistered", Name = nameof(UserIsRegistered))]
#else
        [HttpPost("UserIsRegistered")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Registers User for Application", typeof(APIResponse<bool>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UserIsRegistered(string appId, ApplicationUserRegistrationAPI.RegisterUserParameters parameters)
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
                    if (applicationInfo.ApplicationAPIUrl != null)
                    {
                        var apiResponse = _ApplicationUserRegistrationAPIClient.UserIsRegisteredAsync(appId, parameters, applicationInfo).GetAwaiter().GetResult();
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Updates Application API Configuration Record", typeof(APIResponse<bool>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult RegisterUser(string appId, ApplicationUserRegistrationAPI.RegisterUserParameters parameters)
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
                    if (applicationInfo.ApplicationAPIUrl != null)
                    {
                        var apiResponse = _ApplicationUserRegistrationAPIClient.RegisterUserAsync(appId, parameters, applicationInfo).GetAwaiter().GetResult();
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
