﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Baidu;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Baidu authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class BaiduAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="BaiduAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Baidu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBaidu([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddBaidu(BaiduAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="BaiduAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Baidu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBaidu(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<BaiduAuthenticationOptions> configuration)
        {
            return builder.AddBaidu(BaiduAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="BaiduAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Baidu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Baidu options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBaidu(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<BaiduAuthenticationOptions> configuration)
        {
            return builder.AddBaidu(scheme, BaiduAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="BaiduAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Baidu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Baidu options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBaidu(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<BaiduAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<BaiduAuthenticationOptions, BaiduAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
