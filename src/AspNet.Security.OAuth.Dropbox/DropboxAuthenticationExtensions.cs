/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Dropbox;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class DropboxAuthenticationExtensions {
        public static IApplicationBuilder UseDropboxAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] DropboxAuthenticationOptions options) {
            return app.UseMiddleware<DropboxAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseDropboxAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<DropboxAuthenticationOptions> configuration) {
            var options = new DropboxAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<DropboxAuthenticationMiddleware>(options);
        }
    }
}
