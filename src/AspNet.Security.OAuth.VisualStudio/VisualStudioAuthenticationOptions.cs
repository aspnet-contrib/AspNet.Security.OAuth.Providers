/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.VisualStudio {
    /// <summary>
    /// Defines a set of options used by <see cref="VisualStudioAuthenticationHandler"/>.
    /// </summary>
    public class VisualStudioAuthenticationOptions : OAuthOptions {
        public VisualStudioAuthenticationOptions() {
            AuthenticationScheme = VisualStudioAuthenticationDefaults.AuthenticationScheme;
            DisplayName = VisualStudioAuthenticationDefaults.DisplayName;
            ClaimsIssuer = VisualStudioAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(VisualStudioAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = VisualStudioAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = VisualStudioAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = VisualStudioAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("vso.profile");
        }
    }
}