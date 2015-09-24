/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.GitHub;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder {
    public static class GitHubAuthenticationExtensions {
        public static IApplicationBuilder UseGitHubAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] GitHubAuthenticationOptions options) {
            return app.UseMiddleware<GitHubAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseGitHubAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<GitHubAuthenticationOptions> configuration) {
            var options = new GitHubAuthenticationOptions();
            configuration(options);

            return app.UseGitHubAuthentication(options);
        }
    }
}
