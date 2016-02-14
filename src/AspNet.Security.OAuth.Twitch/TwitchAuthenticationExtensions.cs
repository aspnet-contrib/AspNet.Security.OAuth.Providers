/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Twitch;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder {
    public static class TwitchAuthenticationExtensions {
        public static IApplicationBuilder UseTwitchAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] TwitchAuthenticationOptions options) {
            return app.UseMiddleware<TwitchAuthenticationMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseTwitchAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<TwitchAuthenticationOptions> configuration) {
            var options = new TwitchAuthenticationOptions();
            configuration(options);

            return app.UseTwitchAuthentication(options);
        }
    }
}