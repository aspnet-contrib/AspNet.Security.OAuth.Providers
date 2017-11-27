/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://Untappd.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Untappd;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add  authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class UntappdAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="UntappdAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Untappd authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddUntappd([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddUntappd(UntappdAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="UntappdAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Untappd authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddUntappd(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<UntappdAuthenticationOptions> configuration)
        {
            return builder.AddUntappd(UntappdAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="UntappdAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Untappd authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Untappd options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddUntappd(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<UntappdAuthenticationOptions> configuration)
        {
            return builder.AddUntappd(scheme, UntappdAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="UntappdAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Untappd authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Untappd options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddUntappd(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<UntappdAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<UntappdAuthenticationOptions, UntappdAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
