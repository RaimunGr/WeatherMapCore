using Microsoft.IdentityModel.Tokens;
using System;

namespace App.Api.Utility
{
    public sealed class RaimunSecurityToken : SecurityToken
    {
        private RaimunSecurityToken()
        { }

        public RaimunSecurityToken(
            string id, 
            string issuer, 
            SecurityKey securityKey, 
            SecurityKey signingKey, 
            DateTime validFrom, 
            DateTime validTo
        )
        {
            Id = id;
            Issuer = issuer;
            SecurityKey = securityKey;
            SigningKey = signingKey;
            ValidFrom = validFrom;
            ValidTo = validTo;

        }

        public override string Id { get; }

        public override string Issuer { get; }

        public override SecurityKey SecurityKey { get; }

        public override SecurityKey SigningKey { get ; set; }

        public override DateTime ValidFrom { get; }

        public override DateTime ValidTo { get; }
    }
}
