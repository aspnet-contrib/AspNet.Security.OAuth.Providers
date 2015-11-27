/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.ArcGIS;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class ArcGISAuthenticationExtensions {
        public static IApplicationBuilder UseArcGISAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] ArcGISAuthenticationOptions options) {
            return app.UseMiddleware<ArcGISAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseArcGISAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<ArcGISAuthenticationOptions> configuration) {
            var options = new ArcGISAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<ArcGISAuthenticationMiddleware>(options);
        }
    }
}
