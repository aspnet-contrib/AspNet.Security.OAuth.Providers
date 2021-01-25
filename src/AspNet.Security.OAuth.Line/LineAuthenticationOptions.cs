/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Line.LineAuthenticationConstants;

namespace AspNet.Security.OAuth.Line
{
    /// <summary>
    /// Defines a set of options used by <see cref="LineAuthenticationHandler"/>.
    /// </summary>
    public class LineAuthenticationOptions : OAuthOptions
    {
        public LineAuthenticationOptions()
        {
            ClaimsIssuer = LineAuthenticationDefaults.Issuer;
            CallbackPath = LineAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = LineAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = LineAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = LineAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("profile");
            Scope.Add("openid");
            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
            ClaimActions.MapJsonKey(Claims.PictureUrl, "pictureUrl", "url");
        }

        /// <summary>
        /// Gets or sets the address of the endpoint exposing
        /// the email addresses associated with the logged in user.
        /// </summary>
        public string UserEmailsEndpoint { get; set; } = LineAuthenticationDefaults.UserEmailsEndpoint;
    }
}
