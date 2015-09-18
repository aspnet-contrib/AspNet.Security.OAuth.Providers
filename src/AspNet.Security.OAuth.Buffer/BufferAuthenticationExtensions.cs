/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Buffer;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Builder {
    public static class BufferAuthenticationExtensions {
        public static IServiceCollection ConfigureBufferAuthentication(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<BufferAuthenticationOptions> configuration) {
            return services.Configure(configuration);
        }

        public static IApplicationBuilder UseBufferAuthentication([NotNull] this IApplicationBuilder app) {
            return app.UseMiddleware<BufferAuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseBufferAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<BufferAuthenticationOptions> configuration) {
            return app.UseMiddleware<BufferAuthenticationMiddleware>(
                new ConfigureOptions<BufferAuthenticationOptions>(configuration));
        }
    }
}
