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
    public class ConnectionStringController : ApiControllerBase
    {
        public ConnectionStringController(IServiceProvider services)
            : base(services) { }

#if OperationId
        [HttpGet("GetConnectionString", Name = nameof(GetConnectionString))] // will translate as GetConnectionStringAsync in generated client code
#else
                [HttpGet("GetConnectionString")]
#endif
        [SwaggerResponse((int)HttpStatusCode.OK, "Gets the Connection String value based on appId", typeof(CoreApplicationServicesAPIResponse<ConnectionStringInfo>))]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult<CoreApplicationServicesAPIResponse<ConnectionStringInfo>> GetConnectionString(string databaseName)
        {
            try
            {
                if (_ConnectionStringManager.TryGetConnectionStringInfo(databaseName, out ConnectionStringInfo? connectionStringInfo) && connectionStringInfo is ConnectionStringInfo)
                {
                    return SuccessResponse(connectionStringInfo);
                }
                return ErrorResponse($"No Connection String Found for database name '{databaseName}'", ConnectionStringInfo.Default);
            }
            catch (Exception e)
            {
                return ErrorResponse(e.Message, ConnectionStringInfo.Default);
            }
        }
    }
}
