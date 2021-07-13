/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Notion;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Notion authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class NotionAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="NotionAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Notion authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddNotion([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddNotion(NotionAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="NotionAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Notion authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddNotion(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<NotionAuthenticationOptions> configuration)
        {
            return builder.AddNotion(NotionAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="NotionAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Notion authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Notion options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddNotion(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<NotionAuthenticationOptions> configuration)
        {
            return builder.AddNotion(scheme, NotionAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="NotionAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Notion authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Notion options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddNotion(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<NotionAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<NotionAuthenticationOptions, NotionAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
