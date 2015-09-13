/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Imgur;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class ImgurAuthenticationExtensions {
        public static IServiceCollection ConfigureImgurAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<ImgurAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseImgurAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<ImgurAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseImgurAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<ImgurAuthenticationOptions> configuration) {
            return app.UseMiddleware<ImgurAuthenticationMiddleware>(
                new ConfigureOptions<ImgurAuthenticationOptions>(configuration));
        }
    }
}
