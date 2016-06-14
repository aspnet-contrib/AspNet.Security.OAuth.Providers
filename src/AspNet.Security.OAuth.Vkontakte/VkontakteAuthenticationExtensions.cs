using System;
using AspNet.Security.OAuth.Vkontakte;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add Vkontakte authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class VkontakteAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="VkontakteAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseVkontakteAuthentication(
            [NotNull] this IApplicationBuilder app) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<VkontakteAuthenticationMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="VkontakteAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="VkontakteAuthenticationOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseVkontakteAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] VkontakteAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<VkontakteAuthenticationMiddleware>(Options.Create(options));
        }
    }
}