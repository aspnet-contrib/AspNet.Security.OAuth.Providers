/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.HealthGraph;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class HealthGraphAuthenticationExtensions {
        public static IApplicationBuilder UseHealthGraphAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] HealthGraphAuthenticationOptions options) {
            return app.UseMiddleware<HealthGraphAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseHealthGraphAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<HealthGraphAuthenticationOptions> configuration) {
            var options = new HealthGraphAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<HealthGraphAuthenticationMiddleware>(options);
        }
    }
}
