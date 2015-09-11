/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.DeviantArt;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class DeviantArtAuthenticationExtensions {
        public static IServiceCollection ConfigureDeviantArtAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<DeviantArtAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseDeviantArtAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<DeviantArtAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseDeviantArtAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<DeviantArtAuthenticationOptions> configuration) {
            return app.UseMiddleware<DeviantArtAuthenticationMiddleware>(
                new ConfigureOptions<DeviantArtAuthenticationOptions>(configuration));
        }
    }
}
