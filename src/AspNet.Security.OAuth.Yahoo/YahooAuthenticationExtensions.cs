/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Yahoo;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Yahoo authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class YahooAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="YahooAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Yahoo authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="YahooAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseYahooAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] YahooAuthenticationOptions options) {
            return app.UseMiddleware<YahooAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="YahooAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Yahoo authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="YahooAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseYahooAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<YahooAuthenticationOptions> configuration) {
            var options = new YahooAuthenticationOptions();
            configuration(options);

            return app.UseYahooAuthentication(options);
        }
    }
}