/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Slack;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class SlackAuthenticationExtensions {
        public static IServiceCollection ConfigureSlackAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<SlackAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseSlackAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<SlackAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseSlackAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<SlackAuthenticationOptions> configuration) {
            return app.UseMiddleware<SlackAuthenticationMiddleware>(
                new ConfigureOptions<SlackAuthenticationOptions>(configuration));
        }
    }
}
