/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.SoundCloud;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class SoundCloudAuthenticationExtensions {
        public static IApplicationBuilder UseSoundCloudAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] SoundCloudAuthenticationOptions options) {
            return app.UseMiddleware<SoundCloudAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseSoundCloudAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<SoundCloudAuthenticationOptions> configuration) {
            var options = new SoundCloudAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<SoundCloudAuthenticationMiddleware>(options);
        }
    }
}
