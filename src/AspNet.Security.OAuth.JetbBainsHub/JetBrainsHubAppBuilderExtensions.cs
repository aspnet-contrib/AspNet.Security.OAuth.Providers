using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.JetbBainsHub {
    /// <summary>
    /// Extension methods to add JetBrins Hub authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class JetBrainsHubAppBuilderExtensions {
        /// <summary>
        /// Adds the <see cref="JetBrainsHubMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>,
        /// which enables JetBrains Hub authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseJetBrainsHubAuthentication(this IApplicationBuilder app) {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<JetBrainsHubMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="JetBrainsHubMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>,
        /// which enables JetBrains Hub authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="JetBrainsHubOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseJetBrainsHubAuthentication(this IApplicationBuilder app, JetBrainsHubOptions options) {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return app.UseMiddleware<JetBrainsHubMiddleware>(Options.Create(options));
        }
    }
}