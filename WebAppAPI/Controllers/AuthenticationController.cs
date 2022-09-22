using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using WebAppAPI.Models;

namespace WebAppAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AuthenticationController : ApiControllerBase
    {
        public AuthenticationController(IServiceProvider services) : base(services) { }
        private UserManager _UserManager => Services.GetRequiredService<UserManager>();

#if OperationId
                [HttpPost("Authenticate", Name = nameof(AuthenticateAsync))]
#else
        [HttpPost("Authenticate")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates API Authentication JWT Token", typeof(APIResponse<APIAccessToken>))]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AuthenticateAsync(string appId)
        {
            try
            {
                if (await _UserManager.IsAllowedUserAsync(appId))
                {
                    return SuccessResponse(_UserManager.GetAccessToken(appId));
                }
                return ErrorResponse($"Unauthorized App \"{appId}\"", APIAccessToken.Default);
            }
            catch (Exception ex)
            {
                return ErrorResponse(ex, APIAccessToken.Default);
            }
        }
    }
}
