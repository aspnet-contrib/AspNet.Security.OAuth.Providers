/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Onshape;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class OnshapeAuthenticationExtensions {
        public static IApplicationBuilder UseOnshapeAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] OnshapeAuthenticationOptions options) {
            return app.UseMiddleware<OnshapeAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseOnshapeAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration) {
            var options = new OnshapeAuthenticationOptions();
            configuration(options);

            return app.UseOnshapeAuthentication(options);
        }
    }
}
