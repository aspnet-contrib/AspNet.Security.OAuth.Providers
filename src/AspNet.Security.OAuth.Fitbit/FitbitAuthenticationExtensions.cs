/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Fitbit;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder
{
    public static class FitbitAuthenticationExtensions {
        public static IServiceCollection ConfigureFitbitAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<FitbitAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseFitbitAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<FitbitAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseFitbitAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<FitbitAuthenticationOptions> configuration) {
            return app.UseMiddleware<FitbitAuthenticationMiddleware>(
                new ConfigureOptions<FitbitAuthenticationOptions>(configuration));
        }
    }
}
