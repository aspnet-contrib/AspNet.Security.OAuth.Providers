/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Strava;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Strava authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class StravaAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="StravaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Strava authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddStrava([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddStrava(StravaAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="StravaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Strava authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Strava options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddStrava(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<StravaAuthenticationOptions> configuration)
        {
            return builder.AddStrava(StravaAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="StravaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Strava authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Strava options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddStrava(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<StravaAuthenticationOptions> configuration)
        {
            return builder.AddStrava(scheme, StravaAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="StravaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Strava authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance</param>
        /// <param name="configuration">The delegate used to configure the Strava options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddStrava(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<StravaAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<StravaAuthenticationOptions, StravaAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
