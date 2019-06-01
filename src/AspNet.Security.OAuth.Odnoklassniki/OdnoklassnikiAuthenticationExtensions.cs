/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Odnoklassniki;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Odnoklassniki authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class OdnoklassnikiAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOdnoklassniki([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddOdnoklassniki(OdnoklassnikiAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OAuth 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOdnoklassniki(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            return builder.AddOdnoklassniki(OdnoklassnikiAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Odnoklassniki options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOdnoklassniki(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            return builder.AddOdnoklassniki(scheme, OdnoklassnikiAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Odnoklassniki options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOdnoklassniki(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<OdnoklassnikiAuthenticationOptions, OdnoklassnikiAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
