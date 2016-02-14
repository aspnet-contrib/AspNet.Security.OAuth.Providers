/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Paypal;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class PaypalAuthenticationExtensions {
        public static IApplicationBuilder UsePaypalAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] PaypalAuthenticationOptions options) {
            return app.UseMiddleware<PaypalAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UsePaypalAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<PaypalAuthenticationOptions> configuration) {
            var options = new PaypalAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<PaypalAuthenticationMiddleware>(options);
        }
    }
}
