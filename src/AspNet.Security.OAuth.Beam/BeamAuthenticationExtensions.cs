/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Beam;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class BeamAuthenticationExtensions {
        public static IApplicationBuilder UseBeamAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] BeamAuthenticationOptions options) {
            return app.UseMiddleware<BeamAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseBeamAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<BeamAuthenticationOptions> configuration) {
            var options = new BeamAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<BeamAuthenticationMiddleware>(options);
        }
    }
}
