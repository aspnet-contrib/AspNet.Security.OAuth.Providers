/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Kloudless.KloudlessAuthenticationConstants;

namespace AspNet.Security.OAuth.Kloudless
{
    /// <summary>
    /// Defines a set of options used by <see cref="KloudlessAuthenticationHandler"/>.
    /// </summary>
    public class KloudlessAuthenticationOptions : OAuthOptions
    {
        public KloudlessAuthenticationOptions()
        {
            ClaimsIssuer = KloudlessAuthenticationDefaults.Issuer;
            CallbackPath = KloudlessAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = KloudlessAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = KloudlessAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = KloudlessAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "url");
            ClaimActions.MapJsonKey(Claims.Bio, "bio");
            ClaimActions.MapJsonKey(Claims.Reputation, "reputation");
            ClaimActions.MapJsonKey(Claims.Created, "created");
            ClaimActions.MapJsonKey(Claims.ProExpiration, "pro_expiration");
        }
    }
}
