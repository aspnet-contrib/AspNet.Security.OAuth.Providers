/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Yahoo;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNet.Builder {
    public static class YahooAuthenticationExtensions {
        public static IApplicationBuilder UseYahooAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] YahooAuthenticationOptions options) {
            return app.UseMiddleware<YahooAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseYahooAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<YahooAuthenticationOptions> configuration) {
            var options = new YahooAuthenticationOptions();
            configuration(options);

            return app.UseYahooAuthentication(options);
        }
    }
}