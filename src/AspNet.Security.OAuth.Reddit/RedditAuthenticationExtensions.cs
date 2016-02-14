/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Reddit;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class RedditAuthenticationExtensions {
        public static IApplicationBuilder UseRedditAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] RedditAuthenticationOptions options) {
            return app.UseMiddleware<RedditAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseRedditAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<RedditAuthenticationOptions> configuration) {
            var options = new RedditAuthenticationOptions();
            configuration(options);

            return app.UseRedditAuthentication(options);
        }
    }
}