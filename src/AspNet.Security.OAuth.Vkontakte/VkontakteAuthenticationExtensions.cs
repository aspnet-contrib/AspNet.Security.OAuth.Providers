/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Vkontakte;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Vkontakte authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class VkontakteAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVkontakte([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddVkontakte(VkontakteAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Vkontakte options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVkontakte(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<VkontakteAuthenticationOptions> configuration)
        {
            return builder.AddVkontakte(VkontakteAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Vkontakte options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVkontakte(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme, 
            [NotNull] Action<VkontakteAuthenticationOptions> configuration)
        {
            return builder.AddVkontakte(scheme, VkontakteAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Vkontakte options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVkontakte(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<VkontakteAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<VkontakteAuthenticationOptions, VkontakteAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
