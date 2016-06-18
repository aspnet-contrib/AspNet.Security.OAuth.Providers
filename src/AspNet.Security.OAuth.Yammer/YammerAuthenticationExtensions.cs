/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Microsoft.AspNetCore.Authentication.Yammer;
using Microsoft.Extensions.Options;
using JetBrains.Annotations;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Yammer authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class YammerAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="YammerAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Yammer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseYammerAuthentication(
            [NotNull] this IApplicationBuilder app) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<YammerAuthenticationMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="YammerAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Yammer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="YammerAuthenticationOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseYammerAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] YammerAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<YammerAuthenticationMiddleware>(Options.Create(options));
        }
    }
}