using AuthenticationTokenService.Extensions;
using AuthenticationTokenService.Models;
using DbFacade.DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;

namespace AuthenticationTokenService.Controllers
{

    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IServiceProvider Services { get; set; }
        public ApiControllerBase(IServiceProvider services)
        {
            Services = services;
        }

        protected ActionResult<AuthenticationTokenServiceAPIResponse<T>> SuccessResponse<T>(T item)
            => Ok(AuthenticationTokenServiceAPIResponse<T>.Create(item));
        protected ActionResult<AuthenticationTokenServiceAPIResponse<T>> ErrorResponse<T>(string error, T value)
            => Ok(AuthenticationTokenServiceAPIResponse<T>.Error(error, value));
        protected ActionResult<AuthenticationTokenServiceAPIResponse<T>> ErrorResponse<T>(Exception e, T value)
            => ErrorResponse(e.Message, value);
        protected ActionResult<AuthenticationTokenServiceAPIResponse<T>> ErrorResponse<T>(IDbResponse dbResponse, T value)
            => ErrorResponse($"{dbResponse.ErrorMessage} - {dbResponse.ErrorDetails}", value);
        protected ActionResult<AuthenticationTokenServiceAPIResponse<T>> ErrorResponse<T, TDbDataModel>(IDbResponse<TDbDataModel> dbResponse, T value)
            where TDbDataModel : DbDataModel
            => ErrorResponse($"{dbResponse.ErrorMessage} - {dbResponse.ErrorDetails}", value);

        protected ActionResult<AuthenticationTokenServiceAPIResponse<IEnumerable<T>>> SuccessListResponse<T>(IEnumerable<T> item)
            => SuccessResponse(item);

    }

    
    
}
