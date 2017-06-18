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
    public static class OdnoklassnikiAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddOdnoklassniki([NotNull] this AuthenticationBuilder builder)
            => builder.AddOdnoklassniki(OdnoklassnikiAuthenticationDefaults.AuthenticationScheme, _ => { });

        /// <summary>
        /// Adds the <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="OdnoklassnikiAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddOdnoklassniki(
            [NotNull] this AuthenticationBuilder builder,
            [CanBeNull] Action<OdnoklassnikiAuthenticationOptions> configureOptions)
            => builder.AddOdnoklassniki(OdnoklassnikiAuthenticationDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// Adds the <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="OdnoklassnikiAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddOdnoklassniki(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string authenticationScheme,
            [CanBeNull] Action<OdnoklassnikiAuthenticationOptions> configureOptions)
            => builder.AddOdnoklassniki(authenticationScheme, OdnoklassnikiAuthenticationDefaults.DisplayName, configureOptions);

        /// <summary>
        /// Adds the <see cref="OdnoklassnikiAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Odnoklassniki authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="authenticationScheme">The name of the scheme being added.</param>
        /// <param name="displayName">The display name for the scheme.</param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="OdnoklassnikiAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddOdnoklassniki(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string authenticationScheme,
            [CanBeNull] string displayName,
            [CanBeNull] Action<OdnoklassnikiAuthenticationOptions> configureOptions)
            => builder.AddOAuth<OdnoklassnikiAuthenticationOptions, OdnoklassnikiAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add Odnoklassniki authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class OdnoklassnikiAppBuilderExtensions
    {
        /// <summary>
        /// Obsolete, see https://go.microsoft.com/fwlink/?linkid=845470
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="OdnoklassnikiAuthenticationOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        [Obsolete("See https://go.microsoft.com/fwlink/?linkid=845470", true)]
        public static IApplicationBuilder UseOdnoklassnikiAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] OdnoklassnikiAuthenticationOptions options)
        {
            throw new NotSupportedException("This method is no longer supported, see https://go.microsoft.com/fwlink/?linkid=845470");
        }

        /// <summary>
        /// Obsolete, see https://go.microsoft.com/fwlink/?linkid=845470
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="OdnoklassnikiAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        [Obsolete("See https://go.microsoft.com/fwlink/?linkid=845470", true)]
        public static IApplicationBuilder UseOdnoklassnikiAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<OdnoklassnikiAuthenticationOptions> configuration)
        {
            throw new NotSupportedException("This method is no longer supported, see https://go.microsoft.com/fwlink/?linkid=845470");
        }
    }
}
