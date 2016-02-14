/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Slack;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class SlackAuthenticationExtensions {
        public static IApplicationBuilder UseSlackAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] SlackAuthenticationOptions options) {
            return app.UseMiddleware<SlackAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseSlackAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<SlackAuthenticationOptions> configuration) {
            var options = new SlackAuthenticationOptions();
            configuration(options);

            return app.UseSlackAuthentication(options);
        }
    }
}
