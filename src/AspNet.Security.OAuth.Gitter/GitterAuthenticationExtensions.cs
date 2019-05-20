/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Gitter;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Gitter authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class GitterAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="GitterAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitter authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitter([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddGitter(GitterAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="GitterAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitter authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitter(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<GitterAuthenticationOptions> configuration)
        {
            return builder.AddGitter(GitterAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="GitterAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitter authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Gitter options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitter(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<GitterAuthenticationOptions> configuration)
        {
            return builder.AddGitter(scheme, GitterAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="GitterAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Gitter authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Gitter options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddGitter(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<GitterAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<GitterAuthenticationOptions, GitterAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
