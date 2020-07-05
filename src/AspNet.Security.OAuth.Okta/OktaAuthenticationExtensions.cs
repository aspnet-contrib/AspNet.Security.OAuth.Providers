/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Okta;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Okta authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class OktaAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="OktaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Okta authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOkta([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddOkta(OktaAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="OktaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Okta authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Okta options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOkta(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<OktaAuthenticationOptions> configuration)
        {
            return builder.AddOkta(OktaAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="OktaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Okta authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Okta options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOkta(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<OktaAuthenticationOptions> configuration)
        {
            return builder.AddOkta(scheme, OktaAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="OktaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Okta authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Okta options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOkta(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<OktaAuthenticationOptions> configuration)
        {
            builder.Services.TryAddSingleton<IPostConfigureOptions<OktaAuthenticationOptions>, OktaPostConfigureOptions>();
            return builder.AddOAuth<OktaAuthenticationOptions, OktaAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
