/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Spotify;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder {
    public static class SpotifyAuthenticationExtensions {
        public static IApplicationBuilder UseSpotifyAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] SpotifyAuthenticationOptions options) {
            return app.UseMiddleware<SpotifyAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseSpotifyAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<SpotifyAuthenticationOptions> configuration) {
            var options = new SpotifyAuthenticationOptions();
            configuration(options);

            return app.UseSpotifyAuthentication(options);
        }
    }
}