/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace AspNet.Security.OAuth.Apple;

internal sealed class AppleEmailClaimAction(AppleAuthenticationOptions options) : ClaimAction(ClaimTypes.Email, ClaimValueTypes.String)
{
    public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
    {
        if (!identity.HasClaim((p) => string.Equals(p.Type, ClaimType, StringComparison.OrdinalIgnoreCase)))
        {
            var emailClaim = identity.FindFirst("email");

            if (!string.IsNullOrEmpty(emailClaim?.Value))
            {
                identity.AddClaim(new Claim(ClaimType, emailClaim.Value, ValueType, options.ClaimsIssuer));
            }
        }
    }
}
