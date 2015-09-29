/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;

namespace AspNet.Security.OAuth.DeviantArt {
    /// <summary>
    /// Defines a set of options used by <see cref="DeviantArtAuthenticationHandler"/>.
    /// </summary>
    public class DeviantArtAuthenticationOptions : OAuthOptions {
        public DeviantArtAuthenticationOptions() {
            AuthenticationScheme = DeviantArtAuthenticationDefaults.AuthenticationScheme;
            DisplayName = DeviantArtAuthenticationDefaults.Caption;
            ClaimsIssuer = DeviantArtAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(DeviantArtAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = DeviantArtAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DeviantArtAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DeviantArtAuthenticationDefaults.UserInformationEndpoint;

            SaveTokensAsClaims = false;

            Scope.Add("user");
        }
    }
}
