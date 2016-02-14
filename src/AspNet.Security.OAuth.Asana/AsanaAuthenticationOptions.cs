/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Asana {
    /// <summary>
    /// Defines a set of options used by <see cref="AsanaAuthenticationHandler"/>.
    /// </summary>
    public class AsanaAuthenticationOptions : OAuthOptions {
        public AsanaAuthenticationOptions() {
            AuthenticationScheme = AsanaAuthenticationDefaults.AuthenticationScheme;
            DisplayName = AsanaAuthenticationDefaults.DisplayName;
            ClaimsIssuer = AsanaAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(AsanaAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = AsanaAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = AsanaAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = AsanaAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
