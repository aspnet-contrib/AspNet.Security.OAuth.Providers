/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://Github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Twitch;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {

    public static class TwitchAuthenticationExtensions {

        public static IServiceCollection ConfigureTwitchAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<TwitchAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseTwitchAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<TwitchAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseTwitchAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<TwitchAuthenticationOptions> configuration) {
            return app.UseMiddleware<TwitchAuthenticationMiddleware>(
                new ConfigureOptions<TwitchAuthenticationOptions>(configuration));
        }
    }
}