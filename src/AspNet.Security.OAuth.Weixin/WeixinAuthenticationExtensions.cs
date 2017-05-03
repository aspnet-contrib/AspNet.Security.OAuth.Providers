/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using Microsoft.Extensions.Options;
using JetBrains.Annotations;
using AspNet.Security.OAuth.Weixin;

namespace Microsoft.AspNetCore.Builder
{
    public static class WeixinAuthenticationExtensions
    {
        /// <summary>
        ///  Adds the <see cref="WeixinAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>,
        ///  which enables Weibo authentication capabilities.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] WeixinAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            return app.UseMiddleware<WeixinAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="WeixinAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>,
        /// which enables Weibo authentication capabilities.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<WeixinAuthenticationOptions> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            var options = new WeixinAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<WeixinAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
