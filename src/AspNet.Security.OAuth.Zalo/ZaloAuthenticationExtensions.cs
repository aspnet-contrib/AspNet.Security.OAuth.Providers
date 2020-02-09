/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Zalo;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZaloAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddZalo(options => { });
        }

        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Zalo options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<ZaloAuthenticationOptions> configuration)
        {
            return builder.AddZalo(ZaloAuthenticationDefaults.AuthenticationScheme, ZaloAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Zalo options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<ZaloAuthenticationOptions> configuration)
        {
            return builder.AddZalo(scheme, ZaloAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Zalo options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<ZaloAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<ZaloAuthenticationOptions, ZaloAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
