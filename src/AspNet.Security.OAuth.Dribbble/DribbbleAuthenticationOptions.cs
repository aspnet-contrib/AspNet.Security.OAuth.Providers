/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AspNet.Security.OAuth.Dribbble
{
    /// <summary>
    /// Defines a set of options used by <see cref="DribbbleAuthenticationHandler"/>.
    /// </summary>
    public class DribbbleAuthenticationOptions : OAuthOptions
    {
        public DribbbleAuthenticationOptions()
        {
            ClaimsIssuer = DribbbleAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(DribbbleAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DribbbleAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DribbbleAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DribbbleAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        }
    }
}