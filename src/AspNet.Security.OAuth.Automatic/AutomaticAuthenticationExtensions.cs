/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Automatic;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Automatic authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AutomaticAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="AutomaticAuthenticationMiddleware"/> middleware to the specified
        /// <see cref="IApplicationBuilder"/>, which enables Automatic authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="AutomaticAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAutomaticAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] AutomaticAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<AutomaticAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="AutomaticAuthenticationMiddleware"/> middleware to the specified
        /// <see cref="IApplicationBuilder"/>, which enables Automatic authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="AutomaticAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAutomaticAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<AutomaticAuthenticationOptions> configuration) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new AutomaticAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<AutomaticAuthenticationMiddleware>(Options.Create(options));
        }
    }
}