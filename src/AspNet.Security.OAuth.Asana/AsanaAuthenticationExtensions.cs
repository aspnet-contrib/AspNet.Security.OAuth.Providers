/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Asana;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class AsanaAuthenticationExtensions {
        public static IApplicationBuilder UseAsanaAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] AsanaAuthenticationOptions options) {
            return app.UseMiddleware<AsanaAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseAsanaAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<AsanaAuthenticationOptions> configuration) {
            var options = new AsanaAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<AsanaAuthenticationMiddleware>(options);
        }
    }
}
