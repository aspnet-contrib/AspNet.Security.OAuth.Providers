/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Fitbit;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class FitbitAuthenticationExtensions {
        public static IApplicationBuilder UseFitbitAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] FitbitAuthenticationOptions options) {
            return app.UseMiddleware<FitbitAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseFitbitAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<FitbitAuthenticationOptions> configuration) {
            var options = new FitbitAuthenticationOptions();
            configuration(options);

            return app.UseFitbitAuthentication(options);
        }
    }
}
