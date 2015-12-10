/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Instagram;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class InstagramAuthenticationExtensions {
        public static IApplicationBuilder UseInstagramAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] InstagramAuthenticationOptions options) {
            return app.UseMiddleware<InstagramAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseInstagramAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<InstagramAuthenticationOptions> configuration) {
            var options = new InstagramAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<InstagramAuthenticationMiddleware>(options);
        }
    }
}
