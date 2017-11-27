/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://SoundCloud.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.SoundCloud;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add  authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class SoundCloudAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="SoundCloudAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SoundCloud authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddSoundCloud([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddSoundCloud(SoundCloudAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="SoundCloudAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SoundCloud authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddSoundCloud(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<SoundCloudAuthenticationOptions> configuration)
        {
            return builder.AddSoundCloud(SoundCloudAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="SoundCloudAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SoundCloud authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the SoundCloud options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddSoundCloud(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<SoundCloudAuthenticationOptions> configuration)
        {
            return builder.AddSoundCloud(scheme, SoundCloudAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="SoundCloudAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables SoundCloud authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the SoundCloud options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddSoundCloud(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<SoundCloudAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<SoundCloudAuthenticationOptions, SoundCloudAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
