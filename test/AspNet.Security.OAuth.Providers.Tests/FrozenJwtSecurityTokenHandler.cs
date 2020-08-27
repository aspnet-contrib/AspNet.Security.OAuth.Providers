/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace AspNet.Security.OAuth
{
    internal sealed class FrozenJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        protected override void ValidateLifetime(
            DateTime? notBefore,
            DateTime? expires,
            JwtSecurityToken jwtToken,
            TokenValidationParameters validationParameters)
        {
            // Do not validate the lifetime as the test token has expired
        }
    }
}
