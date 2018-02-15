/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Yammer.YammerAuthenticationConstants;

namespace AspNet.Security.OAuth.Yammer
{
    /// <summary>
    /// Defines a set of options used by <see cref="YammerAuthenticationHandler"/>.
    /// </summary>
    public class YammerAuthenticationOptions : OAuthOptions
    {
        public YammerAuthenticationOptions()
        {
            ClaimsIssuer = YammerAuthenticationDefaults.Issuer;

            CallbackPath = new PathString("/signin-yammer");

            AuthorizationEndpoint = YammerAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = YammerAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = YammerAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(Claims.WebUrl, "web_url");
            ClaimActions.MapJsonKey(Claims.JobTitle, "job_title");
        }
    }
}
