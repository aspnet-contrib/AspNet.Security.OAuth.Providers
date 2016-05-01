/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.GitHub;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods to add GitHub authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class GitHubAuthenticationExtensions {
        /// <summary>
        /// Adds the <see cref="GitHubAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables GitHub authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="GitHubAuthenticationOptions"/> that specifies options for the middleware.</param>        
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseGitHubAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] GitHubAuthenticationOptions options) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<GitHubAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="GitHubAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables GitHub authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="GitHubAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseGitHubAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<GitHubAuthenticationOptions> configuration) {
            if (app == null) {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null) {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new GitHubAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<GitHubAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
