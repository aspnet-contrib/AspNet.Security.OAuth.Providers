/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Slack;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder {
    public static class SlackAuthenticationExtensions {
        public static IApplicationBuilder UseSlackAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] SlackAuthenticationOptions options) {
            return app.UseMiddleware<SlackAuthenticationMiddleware>(options);
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
