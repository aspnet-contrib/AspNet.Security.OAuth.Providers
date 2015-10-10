/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.DeviantArt;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class DeviantArtAuthenticationExtensions {
        public static IApplicationBuilder UseDeviantArtAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] DeviantArtAuthenticationOptions options) {
            return app.UseMiddleware<DeviantArtAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseDeviantArtAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<DeviantArtAuthenticationOptions> configuration) {
            var options = new DeviantArtAuthenticationOptions();
            configuration(options);

            return app.UseDeviantArtAuthentication(options);
        }
    }
}
