/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.HealthGraph;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add HealthGraph authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class HealthGraphAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="HealthGraphAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HealthGraph authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHealthGraph([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddHealthGraph(HealthGraphAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="HealthGraphAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HealthGraph authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHealthGraph(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<HealthGraphAuthenticationOptions> configuration)
        {
            return builder.AddHealthGraph(HealthGraphAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="HealthGraphAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HealthGraph authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the HealthGraph options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHealthGraph(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<HealthGraphAuthenticationOptions> configuration)
        {
            return builder.AddHealthGraph(scheme, HealthGraphAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="HealthGraphAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HealthGraph authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the HealthGraph options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHealthGraph(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<HealthGraphAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<HealthGraphAuthenticationOptions, HealthGraphAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
