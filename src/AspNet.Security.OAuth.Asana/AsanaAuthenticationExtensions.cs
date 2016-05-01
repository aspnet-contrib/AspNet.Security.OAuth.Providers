/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Asana;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Asana authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AsanaAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="AsanaAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="AsanaAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAsanaAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] AsanaAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<AsanaAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="AsanaAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="AsanaAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseAsanaAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<AsanaAuthenticationOptions> configuration) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new AsanaAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<AsanaAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
