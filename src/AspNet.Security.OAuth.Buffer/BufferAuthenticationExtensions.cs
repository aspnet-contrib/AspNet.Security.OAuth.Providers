/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Buffer;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Buffer authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class BufferAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="BufferAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Buffer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="BufferAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseBufferAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] BufferAuthenticationOptions options) {
            return app.UseMiddleware<BufferAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="BufferAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Buffer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="BufferAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseBufferAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<BufferAuthenticationOptions> configuration) {
            var options = new BufferAuthenticationOptions();
            configuration(options);

            return app.UseBufferAuthentication(options);
        }
    }
}
