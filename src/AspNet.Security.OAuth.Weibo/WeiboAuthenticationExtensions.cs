using System;
using Microsoft.Extensions.Options;
using AspNet.Security.OAuth.Weibo;
using JetBrains.Annotations;

namespace Microsoft.AspNetCore.Builder
{
    public static class WeiboAuthenticationExtensions
    {
        /// <summary>
        ///  Adds the <see cref="WeiboAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>,
        ///  which enables Weibo authentication capabilities.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeiboAuthentication([NotNull] this IApplicationBuilder app, [NotNull] WeiboAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<WeiboAuthenticationMiddleware>(Options.Create(options));
        }

        /// <summary>
        /// Adds the <see cref="WeiboAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>,
        /// which enables Weibo authentication capabilities.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeiboAuthentication([NotNull] this IApplicationBuilder app, [NotNull] Action<WeiboAuthenticationOptions> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new WeiboAuthenticationOptions();
            configuration(options);

            return app.UseMiddleware<WeiboAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
