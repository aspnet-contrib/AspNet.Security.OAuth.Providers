/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Basecamp
{
    /// <summary>
    /// Defines a set of options used by <see cref="BasecampAuthenticationHandler"/>.
    /// </summary>
    public class BasecampAuthenticationOptions : OAuthOptions
    {
        public BasecampAuthenticationOptions()
        {
            ClaimsIssuer = BasecampAuthenticationDefaults.Issuer;
            CallbackPath = BasecampAuthenticationDefaults.CallbackPath;
            AuthorizationEndpoint = BasecampAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BasecampAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BasecampAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email_address");
        }
    }
}
