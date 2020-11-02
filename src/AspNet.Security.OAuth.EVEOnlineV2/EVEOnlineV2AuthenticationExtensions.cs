/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using AspNet.Security.OAuth.EVEOnlineV2;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EVEOnlineV2 authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class EVEOnlineV2AuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="EVEOnlineV2AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables EVEOnlineV2 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddEVEOnlineV2([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddEVEOnlineV2(EVEOnlineV2AuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="EVEOnlineV2AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables EVEOnlineV2 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddEVEOnlineV2(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<EVEOnlineV2AuthenticationOptions> configuration)
        {
            return builder.AddEVEOnlineV2(EVEOnlineV2AuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="EVEOnlineV2AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables EVEOnlineV2 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the EVEOnlineV2 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddEVEOnlineV2(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<EVEOnlineV2AuthenticationOptions> configuration)
        {
            return builder.AddEVEOnlineV2(scheme, EVEOnlineV2AuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="EVEOnlineV2AuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables EVEOnlineV2 authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the EVEOnlineV2 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddEVEOnlineV2(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<EVEOnlineV2AuthenticationOptions> configuration)
        {
            builder.Services.TryAddSingleton<JwtSecurityTokenHandler>();

            return builder.AddOAuth<EVEOnlineV2AuthenticationOptions, EVEOnlineV2AuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
