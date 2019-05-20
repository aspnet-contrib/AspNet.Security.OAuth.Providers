/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Twitch;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Twitch authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class TwitchAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="TwitchAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Twitch authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddTwitch([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddTwitch(TwitchAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="TwitchAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Twitch authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddTwitch(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<TwitchAuthenticationOptions> configuration)
        {
            return builder.AddTwitch(TwitchAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="TwitchAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Twitch authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Twitch options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddTwitch(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<TwitchAuthenticationOptions> configuration)
        {
            return builder.AddTwitch(scheme, TwitchAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="TwitchAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Twitch authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Twitch options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddTwitch(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<TwitchAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<TwitchAuthenticationOptions, TwitchAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
