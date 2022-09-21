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
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private ConnectionStringManager _ConnectionStringManager { get; set; }
        public ServiceController(ConnectionStringManager connectionStringManager)
        {
            _ConnectionStringManager = connectionStringManager;
        }


#if OperationId
        [HttpGet("GetConnectionString", Name = nameof(GetConnectionString))]
#else
        [HttpGet("GetConnectionString")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Gets the Connection String value based on appId", typeof(ConnectionStringResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Unknown Error", typeof(ConnectionStringResult))]
        [SwaggerResponse((int)HttpStatusCode.UnprocessableEntity, "No Connection String Found", typeof(ConnectionStringResult))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<ConnectionStringResult> GetConnectionString(string appId)
        {
            try
            {
                if(_ConnectionStringManager.TryGetConnectionString(appId, out string? connectionString) && connectionString is string)
                {
                    return Ok(new ConnectionStringResult(connectionString));
                }
                return UnprocessableEntity(ConnectionStringResult.Error($"No Connection String Found for appId '{appId}'"));
            }
            catch (Exception e)
            {
                return BadRequest(ConnectionStringResult.Error(e.Message));
            }
        }
    }
}
