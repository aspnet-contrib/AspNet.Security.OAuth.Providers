/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Yahoo;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class YahooAuthenticationExtensions {
        public static IServiceCollection ConfigureYahooAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<YahooAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseYahooAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<YahooAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseYahooAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<YahooAuthenticationOptions> configuration) {
            return app.UseMiddleware<YahooAuthenticationMiddleware>(
                new ConfigureOptions<YahooAuthenticationOptions>(configuration));
        }
    }
}