using Infra.ApplicationServices.Utility.Http.Authentication.Models;
using Refit;
using System.Threading.Tasks;

namespace Infra.ApplicationServices.Utility.Http.Authentication.Abstractions
{
    public interface IAuthApi
    {
        [Post("/token/validate")]
        Task<ValidateTokenResponse> ValidateToken(ValidateTokenRequest request);
    }
}
