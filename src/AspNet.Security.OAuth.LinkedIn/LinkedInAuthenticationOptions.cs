/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.LinkedIn
{
    /// <summary>
    /// Defines a set of options used by <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public class LinkedInAuthenticationOptions : OAuthOptions
    {
        public LinkedInAuthenticationOptions()
        {
            AuthenticationScheme = LinkedInAuthenticationDefaults.AuthenticationScheme;
            DisplayName = LinkedInAuthenticationDefaults.DisplayName;
            ClaimsIssuer = LinkedInAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(LinkedInAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = LinkedInAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = LinkedInAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = LinkedInAuthenticationDefaults.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets the list of fields to retrieve from the user information endpoint.
        /// See https://developer.linkedin.com/docs/fields/basic-profile for more information.
        /// </summary>
        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "id",
            "first-name",
            "last-name",
            "formatted-name",
            "email-address"
        };
    }
}