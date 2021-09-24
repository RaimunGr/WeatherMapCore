using Infra.ApplicationServices.Utility.Http.Authentication.Abstractions;
using Infra.ApplicationServices.Utility.Http.Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace App.Api.Utility
{
    public sealed class RaimunSecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly IAuthService _authService;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public RaimunSecurityTokenValidator(IAuthService authService)
        {
            _authService = authService;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(
            string securityToken,
            TokenValidationParameters validationParameters,
            out SecurityToken validatedToken
        )
        {
            if (!CanReadToken(securityToken))
            {
                return Unauthorized(out validatedToken);
            }

            ClaimsPrincipal principal;
            var claims = ExtractClaimsFromToken(securityToken);

            var request = new ValidateTokenRequest(securityToken);
            var rs = _authService.ValidateToken(request).GetAwaiter().GetResult();

            if (!rs.Result.IsValid)
            {
                return Unauthorized(out validatedToken);
            }

            var identity = new ClaimsIdentity(claims, "Basic");
            principal = new ClaimsPrincipal(identity);
            validatedToken = new RaimunSecurityToken(
                claims.FirstOrDefault(c => c.Type.ToLower().Equals("id")).Value,
                claims.FirstOrDefault(c => c.Type.ToLower().Equals("iss")).Value,
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NOT IMPORTANT")),
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("NOT IMPORTANT")),
                DateTime.MinValue,
                DateTime.MaxValue
            );

            return principal;
        }

        private Claim[] ExtractClaimsFromToken(string securityToken)
        {
            var jwt = _tokenHandler.ReadToken(securityToken) as JwtSecurityToken;
            var name = new Claim(
                ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(c => c.Type == "username").Value
            );

            var claims = new List<Claim>();
            claims.AddRange(jwt.Claims);
            claims.Add(name);

            return claims.ToArray();
        }

        private static ClaimsPrincipal Unauthorized(out SecurityToken securityToken)
        {
            securityToken = null;
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        public bool CanValidateToken => true;
        public int MaximumTokenSizeInBytes { get; set; }
    }
}
