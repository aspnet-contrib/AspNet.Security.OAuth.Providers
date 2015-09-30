/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Foursquare;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class FoursquareAuthenticationExtensions {
        public static IApplicationBuilder UseFoursquareAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] FoursquareAuthenticationOptions options) {
            return app.UseMiddleware<FoursquareAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseFoursquareAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<FoursquareAuthenticationOptions> configuration) {
            var options = new FoursquareAuthenticationOptions();
            configuration(options);

            return app.UseFoursquareAuthentication(options);
        }
    }

}
