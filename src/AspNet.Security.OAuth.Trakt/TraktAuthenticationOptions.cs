/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Trakt.TraktAuthenticationConstants;

namespace AspNet.Security.OAuth.Trakt
{
    /// <summary>
    /// Defines a set of options used by <see cref="TraktAuthenticationHandler"/>.
    /// </summary>
    public class TraktAuthenticationOptions : OAuthOptions
    {
        public TraktAuthenticationOptions()
        {
            ClaimsIssuer = TraktAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(TraktAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = TraktAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = TraktAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = TraktAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(Claims.VIP, "vip");
            ClaimActions.MapJsonKey(Claims.VIP_EP, "vip_ep");
            ClaimActions.MapJsonKey(Claims.Private, "private");
        }
    }
}
