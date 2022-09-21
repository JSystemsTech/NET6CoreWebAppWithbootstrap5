using AuthTokenService.Extensions;
using System.Collections.Generic;
using System.ServiceModel;

namespace AuthTokenService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        AuthTokenResponse ToToken(IEnumerable<TokenClaim> claims);

        [OperationContract]
        AuthTokenResponse ParseToken(string tokenStr);
    }
}
