/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AspNet.Security.OAuth.LinkedIn {
    /// <summary>
    /// Defines a set of options used by <see cref="LinkedInAuthenticationHandler"/>.
    /// </summary>
    public class LinkedInAuthenticationOptions : OAuthOptions {
        public LinkedInAuthenticationOptions() {
            AuthenticationScheme = LinkedInAuthenticationDefaults.AuthenticationScheme;
            DisplayName = LinkedInAuthenticationDefaults.DisplayName;
            ClaimsIssuer = LinkedInAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(LinkedInAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = LinkedInAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = LinkedInAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = LinkedInAuthenticationDefaults.UserInformationEndpoint;

            Fields.Add("id");
            Fields.Add("formatted-name");
            Fields.Add("email-address");
        }

        /// <summary>
        /// The list of fields to retrieve from the UserInformationEndpoint.
        /// https://developer.linkedin.com/docs/fields/basic-profile
        /// </summary>
        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}