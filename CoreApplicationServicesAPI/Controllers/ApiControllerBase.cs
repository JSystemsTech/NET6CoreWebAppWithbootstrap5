using ApplicationUserRegistrationAPI;
using CoreApplicationServicesAPI.Models;
using DbFacade.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreApplicationServicesAPI.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ConnectionStringManager _ConnectionStringManager { get; set; }
        protected ApplicationUserRegistrationAPIClient _ApplicationUserRegistrationAPIClient { get; set; }
        public ApiControllerBase(IServiceProvider services)
        {
            _ConnectionStringManager = services.GetRequiredService<ConnectionStringManager>();
            _ApplicationUserRegistrationAPIClient = services.GetRequiredService<ApplicationUserRegistrationAPIClient>();
        }

        protected ActionResult<CoreApplicationServicesAPIResponse<T>> SuccessResponse<T>(T item)
            => Ok(CoreApplicationServicesAPIResponse<T>.Create(item));
        protected ActionResult<CoreApplicationServicesAPIResponse<T>> ErrorResponse<T>(string error, T value)
            => Ok(CoreApplicationServicesAPIResponse<T>.Error(error, value));
        protected ActionResult<CoreApplicationServicesAPIResponse<T>> ErrorResponse<T>(Exception e, T value)
            => ErrorResponse(e.Message, value);
        protected ActionResult<CoreApplicationServicesAPIResponse<T>> ErrorResponse<T>(IDbResponse dbResponse, T value)
            => ErrorResponse($"{dbResponse.ErrorMessage} - {dbResponse.ErrorDetails}", value);
        protected ActionResult<CoreApplicationServicesAPIResponse<T>> ErrorResponse<T, TDbDataModel>(IDbResponse<TDbDataModel> dbResponse, T value)
            where TDbDataModel : DbDataModel
            => ErrorResponse($"{dbResponse.ErrorMessage} - {dbResponse.ErrorDetails}", value);

        protected ActionResult<CoreApplicationServicesAPIResponse<IEnumerable<T>>> SuccessListResponse<T>(IEnumerable<T> item)
            => SuccessResponse(item);

    }
}
