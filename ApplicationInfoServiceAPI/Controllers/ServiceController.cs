using ApplicationInfoServiceAPI.DomainLayer;
using ApplicationInfoServiceAPI.DomainLayer.Models.Data;
using ApplicationInfoServiceAPI.Extensions;
using ApplicationInfoServiceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;

namespace ApplicationInfoServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {

#if OperationId
        [HttpGet("GetApplicationInfo", Name = nameof(GetApplicationInfo))]
#else
        [HttpGet("GetApplicationInfo")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Gets Application Info Record", typeof(ApplicationInfo))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Unknown Error", typeof(ApplicationInfo))]
        [SwaggerResponse((int)HttpStatusCode.UnprocessableEntity, "No Application Info Record Found", typeof(ApplicationInfo))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<ApplicationInfo> GetApplicationInfo(string appId)
        {
            try
            {
                if(DomainFacade.GetApplicationInfo(appId) is ApplicationInfo appInfo)
                {
                    return Ok(appInfo);
                }
                return UnprocessableEntity(default(ApplicationInfo));
            }
            catch (Exception e)
            {
                return BadRequest(default(ApplicationInfo));
            }
        }

#if OperationId
        [HttpGet("ApplicationExists", Name = nameof(GetApplicationInfo))]
#else
        [HttpGet("ApplicationExists")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Checks to see if Application Info Record exists", typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Unknown Error", typeof(bool))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<bool> ApplicationExists(string appId)
        {
            try
            {
                return Ok(DomainFacade.GetApplicationInfo(appId) is ApplicationInfo);
            }
            catch (Exception e)
            {
                return BadRequest(false);
            }
        }
    }
}
