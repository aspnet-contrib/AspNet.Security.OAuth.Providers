/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Vimeo;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class VimeoAuthenticationExtensions {
        public static IApplicationBuilder UseVimeoAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] VimeoAuthenticationOptions options) {
            return app.UseMiddleware<VimeoAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseVimeoAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<VimeoAuthenticationOptions> configuration) {
            var options = new VimeoAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<VimeoAuthenticationMiddleware>(options);
        }
    }
}
