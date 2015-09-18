/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Buffer;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Builder {
    public static class BufferAuthenticationExtensions {
        public static IApplicationBuilder UseBufferAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] BufferAuthenticationOptions options) {
            return app.UseMiddleware<BufferAuthenticationMiddleware>(options);
        }

        public static IApplicationBuilder UseBufferAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<BufferAuthenticationOptions> configuration) {
            var options = new BufferAuthenticationOptions();
            configuration(options);

            return app.UseBufferAuthentication(options);
        }
    }
}
