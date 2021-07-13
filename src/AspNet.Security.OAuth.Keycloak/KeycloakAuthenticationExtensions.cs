/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Keycloak;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Keycloak authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class KeycloakAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="KeycloakAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Keycloak authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddKeycloak([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddKeycloak(KeycloakAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="KeycloakAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Keycloak authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Keycloak options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddKeycloak(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<KeycloakAuthenticationOptions> configuration)
        {
            return builder.AddKeycloak(KeycloakAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="KeycloakAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Keycloak authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Keycloak options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddKeycloak(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<KeycloakAuthenticationOptions> configuration)
        {
            return builder.AddKeycloak(scheme, KeycloakAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="KeycloakAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Keycloak authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Keycloak options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddKeycloak(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<KeycloakAuthenticationOptions> configuration)
        {
            builder.Services.TryAddSingleton<IPostConfigureOptions<KeycloakAuthenticationOptions>, KeycloakPostConfigureOptions>();
            return builder.AddOAuth<KeycloakAuthenticationOptions, KeycloakAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
