/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Amazon;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add Amazon authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AmazonAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="AmazonAuthenticationMiddleware"/> middleware to the specified
        /// <see cref="IApplicationBuilder"/>, which enables Amazon authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="AmazonAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="app"/> or <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static IApplicationBuilder UseAmazonAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] AmazonAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<AmazonAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="AmazonAuthenticationMiddleware"/> middleware to the specified
        /// <see cref="IApplicationBuilder"/>, which enables Amazon authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="AmazonAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="app"/> or <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        public static IApplicationBuilder UseAmazonAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<AmazonAuthenticationOptions> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new AmazonAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<AmazonAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
