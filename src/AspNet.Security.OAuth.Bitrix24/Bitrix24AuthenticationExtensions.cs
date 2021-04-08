/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Bitrix24;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Bitrix24 authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class Bitrix24AuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="Bitrix24AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitrix24 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitrix24([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddBitrix24(Bitrix24AuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="Bitrix24AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitrix24 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitrix24(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<Bitrix24AuthenticationOptions> configuration)
        {
            return builder.AddBitrix24(Bitrix24AuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="Bitrix24AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitrix24 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Bitrix24 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitrix24(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<Bitrix24AuthenticationOptions> configuration)
        {
            return builder.AddBitrix24(scheme, Bitrix24AuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="Bitrix24AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitrix24 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Bitrix24 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitrix24(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<Bitrix24AuthenticationOptions> configuration)
        {
            return builder.AddOAuth<Bitrix24AuthenticationOptions, Bitrix24AuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
