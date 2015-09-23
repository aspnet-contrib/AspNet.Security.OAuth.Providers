/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Imgur;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder {
    public static class ImgurAuthenticationExtensions {
        public static IApplicationBuilder UseImgurAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] ImgurAuthenticationOptions options) {
            return app.UseMiddleware<ImgurAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseImgurAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<ImgurAuthenticationOptions> configuration) {
            var options = new ImgurAuthenticationOptions();
            configuration(options);

            return app.UseImgurAuthentication(options);
        }
    }
}
