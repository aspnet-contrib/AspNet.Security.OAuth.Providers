/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Odnoklassniki;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    public static class OdnoklassnikiAuthenticationExtensions
    {
        public static IApplicationBuilder UseOdnoklassnikiAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] OdnoklassnikiAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<OdnoklassnikiAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseOdnoklassnikiAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new OdnoklassnikiAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<OdnoklassnikiAuthenticationMiddleware>(Options.Create(options));
        }
    }
}