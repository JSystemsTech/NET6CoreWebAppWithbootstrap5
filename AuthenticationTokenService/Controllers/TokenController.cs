using AuthenticationTokenService.Extensions;
using AuthenticationTokenService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace AuthenticationTokenService.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class TokenController : ApiControllerBase
    {
        public TokenController(IServiceProvider services) : base(services) { }

#if OperationId
        [HttpPost("CreateToken", Name = nameof(CreateToken))]
#else
        [HttpPost("CreateToken")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates Authentication Token", typeof(AuthenticationTokenServiceAPIResponse<AuthTokenResponse>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<AuthenticationTokenServiceAPIResponse<AuthTokenResponse>> CreateToken(TokenClaim[] claims)
        {
            try
            {
                return SuccessResponse(claims.ToToken());
            }
            catch (Exception e)
            {
                return ErrorResponse(e, AuthTokenResponse.Default);
            }
        }

#if OperationId
        [HttpPost("ParseToken", Name = nameof(ParseToken))]
#else
        [HttpPost("ParseToken")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Parses and validates Authentication Token", typeof(AuthenticationTokenServiceAPIResponse<AuthTokenResponse>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<AuthenticationTokenServiceAPIResponse<AuthTokenResponse>> ParseToken(string tokenStr)
        {
            try
            {
                return SuccessResponse(tokenStr.ParseToken());
            }
            catch (Exception e)
            {
                return ErrorResponse(e, AuthTokenResponse.Default);
            }
        }

    }
}
