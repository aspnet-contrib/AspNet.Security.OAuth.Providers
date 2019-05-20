/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Foursquare;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Foursquare authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class FoursquareAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="FoursquareAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Foursquare authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddFoursquare([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddFoursquare(FoursquareAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="FoursquareAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Foursquare authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddFoursquare(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<FoursquareAuthenticationOptions> configuration)
        {
            return builder.AddFoursquare(FoursquareAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="FoursquareAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Foursquare authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Foursquare options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddFoursquare(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<FoursquareAuthenticationOptions> configuration)
        {
            return builder.AddFoursquare(scheme, FoursquareAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="FoursquareAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Foursquare authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Foursquare options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddFoursquare(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<FoursquareAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<FoursquareAuthenticationOptions, FoursquareAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
