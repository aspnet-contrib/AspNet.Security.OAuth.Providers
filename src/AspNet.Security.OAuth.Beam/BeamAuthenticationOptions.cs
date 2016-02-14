/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Beam {
    /// <summary>
    /// Defines a set of options used by <see cref="BeamAuthenticationHandler"/>.
    /// </summary>
    public class BeamAuthenticationOptions : OAuthOptions {
        public BeamAuthenticationOptions() {
            AuthenticationScheme = BeamAuthenticationDefaults.AuthenticationScheme;
            DisplayName = BeamAuthenticationDefaults.DisplayName;
            ClaimsIssuer = BeamAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(BeamAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BeamAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BeamAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BeamAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
