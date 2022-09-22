using AuthenticationTokenService.Extensions;
using AuthenticationTokenService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Net;
using System.Xml.Linq;

namespace AuthenticationTokenService.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class SSOController : ApiControllerBase
    {
        public SSOController(IServiceProvider services) : base(services) { }

#if OperationId
        [HttpPost("CreateSSOToken", Name = nameof(CreateSSOToken))]
#else
        [HttpPost("CreateSSOToken")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Creates Authentication Token", typeof(APIResponse<AuthTokenResponse>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult CreateSSOToken(SSOTokenParameters parameters)
        {
            try
            {
                var claims = new TokenClaim[] {
                    new TokenClaim(){ Name="EDIPI",Value = parameters.EDIPI },
                    new TokenClaim(){ Name="Email",Value = parameters.Email },
                    new TokenClaim(){ Name="FirstName",Value = parameters.FirstName },
                    new TokenClaim(){ Name="LastName",Value = parameters.LastName },
                    new TokenClaim(){ Name="MiddleInitial",Value = parameters.MiddleInitial }
                };
                string? errorMessage =
                        string.IsNullOrWhiteSpace(parameters.EDIPI) ? "Missing EDIPI" :
                        string.IsNullOrWhiteSpace(parameters.Email) ? "Missing Email" :
                        string.IsNullOrWhiteSpace(parameters.FirstName) ? "Missing First Name" :
                        string.IsNullOrWhiteSpace(parameters.LastName) ? "Missing Last Name" :
                        string.IsNullOrWhiteSpace(parameters.MiddleInitial) ? "Missing Middle Initial" : null;


                return errorMessage is string errStr ? ErrorResponse(errStr, AuthTokenResponse.Default) :
                    SuccessResponse(claims.ToToken());
            }
            catch (Exception e)
            {
                return ErrorResponse(e, AuthTokenResponse.Default);
            }
        }
#if OperationId
        [HttpPost("ParseSSOToken", Name = nameof(ParseSSOToken))]
#else
        [HttpPost("ParseSSOToken")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Parses and validates SSO Token", typeof(APIResponse<SSOTokenDataResponse>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult ParseSSOToken(string ssoToken)
        {
            try
            {
                if (ssoToken.ParseToken() is AuthTokenResponse data)
                {
                    var edipi = data.Claims.FirstOrDefault(c => c.Name == "EDIPI") is TokenClaim tc ? tc.Value : "";
                    var email = data.Claims.FirstOrDefault(c => c.Name == "Email") is TokenClaim tc1 ? tc1.Value : "";
                    var firstName = data.Claims.FirstOrDefault(c => c.Name == "FirstName") is TokenClaim tc2 ? tc2.Value : "";
                    var lastName = data.Claims.FirstOrDefault(c => c.Name == "LastName") is TokenClaim tc3 ? tc3.Value : "";
                    var middleInitial = data.Claims.FirstOrDefault(c => c.Name == "MiddleInitial") is TokenClaim tc4 ? tc4.Value : "";

                    string? errorMessage =
                        !data.IsValid ? "Invalid SSO Token" :
                        data.Claims.Count() == 0 ? "No Claims found on token" :
                        string.IsNullOrWhiteSpace(edipi) ? "Missing EDIPI" :
                        string.IsNullOrWhiteSpace(email) ? "Missing Email" :
                        string.IsNullOrWhiteSpace(firstName) ? "Missing First Name" :
                        string.IsNullOrWhiteSpace(lastName) ? "Missing Last Name" :
                        string.IsNullOrWhiteSpace(middleInitial) ? "Missing Middle Initial" : null;


                    return errorMessage is string errStr ? ErrorResponse(errStr, SSOTokenDataResponse.Default) :
                        SuccessResponse(new SSOTokenDataResponse(edipi, email, firstName, lastName, middleInitial));
                }
                return ErrorResponse("missing SSO Token", SSOTokenDataResponse.Default);
            }
            catch (Exception e)
            {
                return ErrorResponse(e, SSOTokenDataResponse.Default);
            }
        }
    }
}
