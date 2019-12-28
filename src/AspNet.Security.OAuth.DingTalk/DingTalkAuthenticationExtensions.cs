/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.DingTalk;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add DingTalk authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class DingTalkAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="DingTalkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables DingTalk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDingTalk([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddDingTalk(DingTalkAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="DingTalkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables DingTalk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDingTalk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<DingTalkAuthenticationOptions> configuration)
        {
            return builder.AddDingTalk(DingTalkAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="DingTalkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables DingTalk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the DingTalk options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDingTalk(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<DingTalkAuthenticationOptions> configuration)
        {
            return builder.AddDingTalk(scheme, DingTalkAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="DingTalkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables DingTalk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the DingTalk options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDingTalk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<DingTalkAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<DingTalkAuthenticationOptions, DingTalkAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}

