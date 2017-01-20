/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;

namespace AspNet.Security.OAuth.Extensions
{
    public static class ClaimsExtensions
    {
        public static ClaimsIdentity AddOptionalClaim(this ClaimsIdentity identity, string name, string value, string issuer)
        {
            return AddOptionalClaim(identity, name, value, ClaimValueTypes.String, issuer);
        }

        public static ClaimsIdentity AddOptionalClaim(this ClaimsIdentity identity, string name, string value, string type, string issuer)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            // Don't update the identity if the claim cannot be safely added.
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return identity;
            }

            identity.AddClaim(new Claim(name, value, type ?? ClaimValueTypes.String, issuer ?? ClaimsIdentity.DefaultIssuer));
            return identity;
        }
    }
}
