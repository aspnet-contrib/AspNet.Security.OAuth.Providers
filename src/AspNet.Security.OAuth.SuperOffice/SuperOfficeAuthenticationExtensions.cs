/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.IdentityModel.Tokens.Jwt;
using AspNet.Security.OAuth.SuperOffice;
using AspNet.Security.OAuth.SuperOffice.Implementation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add SuperOffice authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class SuperOfficeAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="SuperOfficeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SuperOffice authentication capabilities.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddSuperOffice([NotNull] this AuthenticationBuilder builder)
            => builder.AddSuperOffice(SuperOfficeAuthenticationDefaults.AuthenticationScheme, _ => { });

        /// <summary>
        /// Adds <see cref="SuperOfficeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SuperOffice authentication capabilities.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">The delegate used to configure the OAuth 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddSuperOffice(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<SuperOfficeAuthenticationOptions> configuration)
            => builder.AddSuperOffice(SuperOfficeAuthenticationDefaults.AuthenticationScheme, configuration);

        /// <summary>
        /// Adds <see cref="SuperOfficeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SuperOffice authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="authenticationScheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the SuperOffice options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddSuperOffice(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string authenticationScheme,
            [NotNull] Action<SuperOfficeAuthenticationOptions> configuration)
            => builder.AddSuperOffice(authenticationScheme, SuperOfficeAuthenticationDefaults.DisplayName, configuration);

        /// <summary>
        /// Adds <see cref="SuperOfficeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SuperOffice authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="authenticationScheme">The authentication scheme associated with this instance.</param>
        /// <param name="displayName">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the SuperOffice options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddSuperOffice(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string authenticationScheme,
            [NotNull] string displayName,
            [NotNull] Action<SuperOfficeAuthenticationOptions> configuration)
        {
            builder.Services.TryAddSingleton<SuperOfficeIdTokenValidator, DefaultSuperOfficeIdTokenValidator>();
            builder.Services.TryAddSingleton<SuperOfficeAuthenticationConfigurationManager, DefaultSuperOfficeConfigurationManager>();
            builder.Services.TryAddSingleton<JwtSecurityTokenHandler>();

            return builder.AddOAuth<SuperOfficeAuthenticationOptions, SuperOfficeAuthenticationHandler>(authenticationScheme, displayName, configuration);
        }
    }
}
