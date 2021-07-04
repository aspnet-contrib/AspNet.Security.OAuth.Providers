/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.AdobeIO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add AdobeIO authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AdobeIOAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="AdobeIOAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables AdobeIO authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAdobeIO([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddAdobeIO(AdobeIOAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="AdobeIOAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables AdobeIO authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAdobeIO(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<AdobeIOAuthenticationOptions> configuration)
        {
            return builder.AddAdobeIO(AdobeIOAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AdobeIOAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables AdobeIO authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the AdobeIO options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAdobeIO(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<AdobeIOAuthenticationOptions> configuration)
        {
            return builder.AddAdobeIO(scheme, AdobeIOAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AdobeIOAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables AdobeIO authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the AdobeIO options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAdobeIO(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<AdobeIOAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AdobeIOAuthenticationOptions, AdobeIOAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
