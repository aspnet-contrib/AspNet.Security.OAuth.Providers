/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.WorkWeixin;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Weixin authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class WorkWeixinAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="WorkWeixinAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Weixin authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddWorkWeixin([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddWorkWeixin(WorkWeixinAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="WorkWeixinAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Weixin authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddWorkWeixin(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<WorkWeixinAuthenticationOptions> configuration)
        {
            return builder.AddWorkWeixin(WorkWeixinAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="WorkWeixinAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Weixin authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Weixin options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddWorkWeixin(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<WorkWeixinAuthenticationOptions> configuration)
        {
            return builder.AddWorkWeixin(scheme, WorkWeixinAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="WorkWeixinAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Weixin authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Weixin options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddWorkWeixin(
             [NotNull] this AuthenticationBuilder builder,
             [NotNull] string scheme,
             string caption,
             [NotNull] Action<WorkWeixinAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<WorkWeixinAuthenticationOptions, WorkWeixinAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
