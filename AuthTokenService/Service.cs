using AuthTokenService.Extensions;
using System.Collections.Generic;

namespace AuthTokenService
{
    public class Service : IService
    {
        public AuthTokenResponse ToToken(IEnumerable<TokenClaim> claims) => claims.ToToken();
        public AuthTokenResponse ParseToken(string tokenStr) => tokenStr.ParseToken();
    }
}
