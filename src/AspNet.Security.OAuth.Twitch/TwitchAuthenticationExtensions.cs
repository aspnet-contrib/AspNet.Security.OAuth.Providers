/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Twitch;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Twitch authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class TwitchAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="TwitchAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Twitch authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="TwitchAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseTwitchAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] TwitchAuthenticationOptions options) {
            return app.UseMiddleware<TwitchAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="TwitchAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Twitch authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="TwitchAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseTwitchAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<TwitchAuthenticationOptions> configuration) {
            var options = new TwitchAuthenticationOptions();
            configuration(options);

            return app.UseTwitchAuthentication(options);
        }
    }
}