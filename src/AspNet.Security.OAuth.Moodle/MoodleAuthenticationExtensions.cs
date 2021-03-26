/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using AspNet.Security.OAuth.Moodle;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Moodle authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class MoodleAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="MoodleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Moodle authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddMoodle([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddMoodle(MoodleAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        /// <summary>
        /// Adds <see cref="MoodleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Moodle authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddMoodle(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<MoodleAuthenticationOptions> configuration)
        {
            return builder.AddMoodle(MoodleAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="MoodleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Moodle authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Moodle options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMoodle(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<MoodleAuthenticationOptions> configuration)
        {
            return builder.AddMoodle(scheme, MoodleAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="MoodleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Moodle authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Moodle options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMoodle(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] string caption,
            [NotNull] Action<MoodleAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<MoodleAuthenticationOptions, MoodleAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
