using ConnectionStringAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace ConnectionStringAPI.Controllers
{
    [Route("api/[controller]")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates API Authentication JWT Token", typeof(AccessToken))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Unauthorized User Access", typeof(AccessToken))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Unknown Error", typeof(AccessToken))]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<AccessToken> Authenticate(string appId)
        {
            try
            {
                if (_UserManager.IsAllowedUser(appId))
                {
                    return Ok(_UserManager.GetAccessToken(appId));
                }

                return Unauthorized(AccessToken.Error($"Unauthorized App \"{appId}\""));
            }
            catch
            {
                return BadRequest(AccessToken.Error("An error occurred in generating the token"));
            }
        }
    }
}
