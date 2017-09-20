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
    /// Extension methods to add Vkontakte authentication handler.
    /// </summary>
    public static class VkontakteAuthenticationOptionsExtensions
    {
        /// <summary>
        /// Adds the <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddVkontakte([NotNull] this AuthenticationBuilder builder)
            => builder.AddVkontakte(VkontakteAuthenticationDefaults.AuthenticationScheme, _ => { });

        /// <summary>
        /// Adds the <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="VkontakteAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddVkontakte(
            [NotNull] this AuthenticationBuilder builder,
            [CanBeNull] Action<VkontakteAuthenticationOptions> configureOptions)
            => builder.AddVkontakte(VkontakteAuthenticationDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// Adds the <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="VkontakteAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddVkontakte(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string authenticationScheme,
            [CanBeNull] Action<VkontakteAuthenticationOptions> configureOptions)
            => builder.AddVkontakte(authenticationScheme, VkontakteAuthenticationDefaults.DisplayName, configureOptions);

        /// <summary>
        /// Adds the <see cref="VkontakteAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Vkontakte authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="authenticationScheme">The name of the scheme being added.</param>
        /// <param name="displayName">The display name for the scheme.</param>
        /// <param name="configureOptions">An action delegate to configure the provided <see cref="VkontakteAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddVkontakte(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string authenticationScheme,
            [CanBeNull] string displayName,
            [CanBeNull] Action<VkontakteAuthenticationOptions> configureOptions)
            => builder.AddOAuth<VkontakteAuthenticationOptions, VkontakteAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
    }
}

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add Vkontakte authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class VkontakteAppBuilderExtensions
    {
        /// <summary>
        /// Obsolete, see https://go.microsoft.com/fwlink/?linkid=845470
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the handler to.</param>
        /// <param name="options">A <see cref="VkontakteAuthenticationOptions"/> that specifies options for the handler.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        [Obsolete("See https://go.microsoft.com/fwlink/?linkid=845470", true)]
        public static IApplicationBuilder UseVkontakteAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] VkontakteAuthenticationOptions options)
        {
            throw new NotSupportedException("This method is no longer supported, see https://go.microsoft.com/fwlink/?linkid=845470");
        }

        /// <summary>
        /// Obsolete, see https://go.microsoft.com/fwlink/?linkid=845470
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the handler to.</param>
        /// <param name="configuration">An action delegate to configure the provided <see cref="VkontakteAuthenticationOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        [Obsolete("See https://go.microsoft.com/fwlink/?linkid=845470", true)]
        public static IApplicationBuilder UseVkontakteAuthentication(
            [NotNull] this IApplicationBuilder app,
            [NotNull] Action<VkontakteAuthenticationOptions> configuration)
        {
            throw new NotSupportedException("This method is no longer supported, see https://go.microsoft.com/fwlink/?linkid=845470");
        }
    }
}
