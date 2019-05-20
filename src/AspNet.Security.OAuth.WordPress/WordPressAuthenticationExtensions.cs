/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.WordPress;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add WordPress authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class WordPressAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="WordPressAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WordPress authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddWordPress([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddWordPress(WordPressAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="WordPressAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WordPress authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddWordPress(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<WordPressAuthenticationOptions> configuration)
        {
            return builder.AddWordPress(WordPressAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="WordPressAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WordPress authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the WordPress options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddWordPress(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<WordPressAuthenticationOptions> configuration)
        {
            return builder.AddWordPress(scheme, WordPressAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="WordPressAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WordPress authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the WordPress options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddWordPress(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<WordPressAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<WordPressAuthenticationOptions, WordPressAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
