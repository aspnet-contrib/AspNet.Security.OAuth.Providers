/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Spotify {
    /// <summary>
    /// Defines a set of options used by <see cref="SpotifyAuthenticationHandler"/>.
    /// </summary>
    public class SpotifyAuthenticationOptions : OAuthOptions {
        public SpotifyAuthenticationOptions() {
            AuthenticationScheme = SpotifyAuthenticationDefaults.AuthenticationScheme;
            DisplayName = SpotifyAuthenticationDefaults.DisplayName;
            ClaimsIssuer = SpotifyAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(SpotifyAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = SpotifyAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = SpotifyAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = SpotifyAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}