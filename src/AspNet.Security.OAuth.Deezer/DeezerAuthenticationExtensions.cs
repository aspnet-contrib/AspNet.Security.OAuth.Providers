/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Deezer;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Deezer authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class DeezerAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="DeezerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Deezer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddDeezer([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddDeezer(DeezerAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        /// <summary>
        /// Adds <see cref="DeezerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Deezer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Deezer options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddDeezer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<DeezerAuthenticationOptions> configuration)
        {
            return builder.AddDeezer(DeezerAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="DeezerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Deezer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Deezer options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDeezer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<DeezerAuthenticationOptions> configuration)
        {
            return builder.AddDeezer(scheme, DeezerAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="DeezerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Deezer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Deezer options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDeezer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<DeezerAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<DeezerAuthenticationOptions, DeezerAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
