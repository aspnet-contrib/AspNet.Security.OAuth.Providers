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
    /// <summary>
    /// Extension methods to add Paypal authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class PaypalAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="PaypalAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Paypal authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="PaypalAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UsePaypalAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] PaypalAuthenticationOptions options) {
            return app.UseMiddleware<PaypalAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="PaypalAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Paypal authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="PaypalAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UsePaypalAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<PaypalAuthenticationOptions> configuration) {
            var options = new PaypalAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<PaypalAuthenticationMiddleware>(options);
        }
    }
}
