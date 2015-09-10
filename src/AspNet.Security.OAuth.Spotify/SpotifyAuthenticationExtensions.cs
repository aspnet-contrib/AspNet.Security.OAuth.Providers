/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Spotify;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class SpotifyAuthenticationExtensions {
        public static IServiceCollection ConfigureSpotifyAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<SpotifyAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseSpotifyAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<SpotifyAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseSpotifyAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<SpotifyAuthenticationOptions> configuration) {
            return app.UseMiddleware<SpotifyAuthenticationMiddleware>(
                new ConfigureOptions<SpotifyAuthenticationOptions>(configuration));
        }
    }
}