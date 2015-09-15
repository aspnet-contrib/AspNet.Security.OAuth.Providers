/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Reddit;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class RedditAuthenticationExtensions {
        public static IServiceCollection ConfigureRedditAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<RedditAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseRedditAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<RedditAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseRedditAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<RedditAuthenticationOptions> configuration) {
            return app.UseMiddleware<RedditAuthenticationMiddleware>(
                new ConfigureOptions<RedditAuthenticationOptions>(configuration));
        }
    }
}