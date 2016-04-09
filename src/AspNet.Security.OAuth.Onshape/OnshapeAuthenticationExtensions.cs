/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Onshape;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Onshape authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class OnshapeAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="OnshapeAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Onshape authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="OnshapeAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseOnshapeAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] OnshapeAuthenticationOptions options) {
            return app.UseMiddleware<OnshapeAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="OnshapeAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Onshape authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="OnshapeAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseOnshapeAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration) {
            var options = new OnshapeAuthenticationOptions();
            configuration(options);

            return app.UseOnshapeAuthentication(options);
        }
    }
}
