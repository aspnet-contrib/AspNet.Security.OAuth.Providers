/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.GitLab;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder{
    public static class GitLabAuthenticationExtensions {
        public static IApplicationBuilder UseGitLabAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] GitLabAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<GitLabAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseGitLabAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<GitLabAuthenticationOptions> configuration) {
            var options = new GitLabAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<GitLabAuthenticationMiddleware>(options);
        }
    }
}
