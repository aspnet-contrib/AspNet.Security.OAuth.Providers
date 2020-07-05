/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Patreon.PatreonAuthenticationConstants;

namespace AspNet.Security.OAuth.Patreon
{
    /// <summary>
    /// Defines a set of options used by <see cref="PatreonAuthenticationHandler"/>.
    /// </summary>
    public class PatreonAuthenticationOptions : OAuthOptions
    {
        public PatreonAuthenticationOptions()
        {
            ClaimsIssuer = PatreonAuthenticationDefaults.Issuer;
            CallbackPath = PatreonAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = PatreonAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = PatreonAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = PatreonAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("identity");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonSubKey(ClaimTypes.Email, "attributes", "email");
            ClaimActions.MapJsonSubKey(ClaimTypes.GivenName, "attributes", "first_name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Name, "attributes", "full_name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Surname, "attributes", "last_name");
            ClaimActions.MapJsonSubKey(ClaimTypes.Webpage, "attributes", "url");
            ClaimActions.MapJsonSubKey(Claims.Avatar, "attributes", "thumb_url");
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "first_name",
            "full_name",
            "last_name",
            "thumb_url",
            "url",
        };

        /// <summary>
        /// Gets the list of related data to include from the user information endpoint.
        /// </summary>
        public ISet<string> Includes { get; } = new HashSet<string>();
    }
}
