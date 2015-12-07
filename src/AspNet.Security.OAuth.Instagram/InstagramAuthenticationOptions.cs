/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.Instagram {
    /// <summary>
    /// Defines a set of options used by <see cref="InstagramAuthenticationHandler"/>.
    /// </summary>
    public class InstagramAuthenticationOptions : OAuthOptions {
        public bool SignedRequestsEnforced { get; set; } = false;

        public InstagramAuthenticationOptions() {
            AuthenticationScheme = InstagramAuthenticationDefaults.AuthenticationScheme;
            DisplayName = InstagramAuthenticationDefaults.DisplayName;
            ClaimsIssuer = InstagramAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(InstagramAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = InstagramAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = InstagramAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = InstagramAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("basic");

            SaveTokensAsClaims = false;
        }
    }
}
