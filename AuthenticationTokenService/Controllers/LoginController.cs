using AuthenticationTokenService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace AuthenticationTokenService.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class LoginController : ApiControllerBase
    {
        public LoginController(IServiceProvider services) : base(services) { }
        private UserManager _UserManager => Services.GetRequiredService<UserManager>();
        
        #if OperationId
                [HttpPost("Authenticate", Name = nameof(Authenticate))]
        #else
                [HttpPost("Authenticate")]
        #endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates API Authentication JWT Token", typeof(AuthenticationTokenServiceAPIResponse<AuthenticationTokenServiceAccessToken>))]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<AuthenticationTokenServiceAPIResponse<AuthenticationTokenServiceAccessToken>>> Authenticate(string appId)
        {
            try
            {
                if (await _UserManager.IsAllowedUserAsync(appId))
                {
                    return SuccessResponse(_UserManager.GetAccessToken(appId));
                }
                return ErrorResponse($"Unauthorized App \"{appId}\"", AuthenticationTokenServiceAccessToken.Default);
            }
            catch(Exception ex)
            {
                return ErrorResponse(ex, AuthenticationTokenServiceAccessToken.Default);
            }
        }
    }
}
