/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Amazon;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Amazon authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AmazonAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="AmazonAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Amazon authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAmazon([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddAmazon(AmazonAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="AmazonAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Amazon authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Amazon options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAmazon(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<AmazonAuthenticationOptions> configuration)
        {
            return builder.AddAmazon(AmazonAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AmazonAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Amazon authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Amazon options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAmazon(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<AmazonAuthenticationOptions> configuration)
        {
            return builder.AddAmazon(scheme, AmazonAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AmazonAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Amazon authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Amazon options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAmazon(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<AmazonAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AmazonAuthenticationOptions, AmazonAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
