/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.EVEOnline;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class EVEOnlineAuthenticationExtensions {
        public static IApplicationBuilder UseEVEOnlineAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] EVEOnlineAuthenticationOptions options) {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<EVEOnlineAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseEVEOnlineAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<EVEOnlineAuthenticationOptions> configuration) {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new EVEOnlineAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<EVEOnlineAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
