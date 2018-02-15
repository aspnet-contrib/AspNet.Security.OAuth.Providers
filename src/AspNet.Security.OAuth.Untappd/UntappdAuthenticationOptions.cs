/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Untappd.UntappdAuthenticationConstants;

namespace AspNet.Security.OAuth.Untappd
{
    /// <summary>
    /// Defines a set of options used by <see cref="UntappdAuthenticationHandler"/>.
    /// </summary>
    public class UntappdAuthenticationOptions : OAuthOptions
    {
        public UntappdAuthenticationOptions()
        {
            ClaimsIssuer = UntappdAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(UntappdAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = UntappdAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = UntappdAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = UntappdAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "user_name");
            ClaimActions.MapJsonKey(ClaimTypes.Webpage, "url");
            ClaimActions.MapJsonKey(Claims.Avatar, "user_avatar");
        }
    }
}
