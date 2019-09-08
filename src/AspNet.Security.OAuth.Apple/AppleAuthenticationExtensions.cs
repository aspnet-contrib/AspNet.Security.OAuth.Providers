﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Apple;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Sign In with Apple authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AppleAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="AppleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Apple authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddApple([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddApple(AppleAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="AppleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Apple authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Apple options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddApple(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<AppleAuthenticationOptions> configuration)
        {
            return builder.AddApple(AppleAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AppleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Apple authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Apple options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddApple(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<AppleAuthenticationOptions> configuration)
        {
            return builder.AddApple(scheme, AppleAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AppleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Apple authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Apple options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddApple(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<AppleAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AppleAuthenticationOptions, AppleAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
