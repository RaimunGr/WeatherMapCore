namespace Infra.ApplicationServices.Utility.Http.Authentication.Models
{
    public sealed class ValidateTokenRequest
    {
        public ValidateTokenRequest(string token)
        {
            Token = token;
        }

        public string Token { get; private set; }
    }
}
