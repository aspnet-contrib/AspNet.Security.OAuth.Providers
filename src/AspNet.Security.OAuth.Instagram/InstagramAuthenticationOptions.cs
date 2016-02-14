/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Instagram {
    /// <summary>
    /// Defines a set of options used by <see cref="InstagramAuthenticationHandler"/>.
    /// </summary>
    public class InstagramAuthenticationOptions : OAuthOptions {
        public InstagramAuthenticationOptions() {
            AuthenticationScheme = InstagramAuthenticationDefaults.AuthenticationScheme;
            DisplayName = InstagramAuthenticationDefaults.DisplayName;
            ClaimsIssuer = InstagramAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(InstagramAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = InstagramAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = InstagramAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = InstagramAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("basic");
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether the userinfo requests
        /// should be signed before being sent to the Instagram API.
        /// Enabling this option is recommended when the client application
        /// has been configured to enforce signed requests.
        /// </summary>
        public bool UseSignedRequests { get; set; }
    }
}
