using Infra.ApplicationServices.Utility.Http.Authentication.Abstractions;
using Infra.ApplicationServices.Utility.Http.Authentication.Models;
using Refit;
using System.Threading.Tasks;

namespace Infra.ApplicationServices.Utility.Http.Authentication.Implementations
{
    public sealed class AuthService : IAuthService
    {
        private readonly IAuthApi _api;

        public AuthService(string baseAddress)
        {
            _api = RestService.For<IAuthApi>(baseAddress);
        }

        public Task<ValidateTokenResponse> ValidateToken(ValidateTokenRequest request)
        {
            return _api.ValidateToken(request);
        }
    }
}
