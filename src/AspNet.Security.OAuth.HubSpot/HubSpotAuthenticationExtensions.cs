/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.HubSpot;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add HubSpot authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class HubSpotAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="HubSpotAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HubSpot authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHubSpot([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddHubSpot(HubSpotAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="HubSpotAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HubSpot authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHubSpot(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<HubSpotAuthenticationOptions> configuration)
        {
            return builder.AddHubSpot(HubSpotAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="HubSpotAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HubSpot authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the HubSpot options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHubSpot(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<HubSpotAuthenticationOptions> configuration)
        {
            return builder.AddHubSpot(scheme, HubSpotAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="HubSpotAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables HubSpot authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the HubSpot options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddHubSpot(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<HubSpotAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<HubSpotAuthenticationOptions, HubSpotAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
