using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Net;
using System.Xml.Linq;
using CoreApplicationServicesAPI;
using WebAppAPI.Models;

namespace WebAppAPI.Controllers
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Registers User for Application", typeof(APIResponse<bool>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UserIsRegistered(RegisterUserParameters parameters)
        {
            try
            {
                return SuccessResponse(true);
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
        public IActionResult RegisterUser(RegisterUserParameters parameters)
        {
            try
            {
                return SuccessResponse(true);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, false);
            }
        }


    }
}
