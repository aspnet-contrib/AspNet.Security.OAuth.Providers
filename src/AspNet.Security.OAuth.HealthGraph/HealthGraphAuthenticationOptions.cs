/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.HealthGraph {
    /// <summary>
    /// Defines a set of options used by <see cref="HealthGraphAuthenticationHandler"/>.
    /// </summary>
    public class HealthGraphAuthenticationOptions : OAuthOptions {
        public HealthGraphAuthenticationOptions() {
            AuthenticationScheme = HealthGraphAuthenticationDefaults.AuthenticationScheme;
            DisplayName = HealthGraphAuthenticationDefaults.DisplayName;
            ClaimsIssuer = HealthGraphAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(HealthGraphAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = HealthGraphAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = HealthGraphAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = HealthGraphAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;
        }
    }
}
