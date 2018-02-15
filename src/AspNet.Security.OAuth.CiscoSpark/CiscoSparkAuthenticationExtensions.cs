/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.CiscoSpark;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add CiscoSpark authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class CiscoSparkAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="CiscoSparkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables CiscoSpark authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddCiscoSpark([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddCiscoSpark(CiscoSparkAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="CiscoSparkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables CiscoSpark authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddCiscoSpark(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<CiscoSparkAuthenticationOptions> configuration)
        {
            return builder.AddCiscoSpark(CiscoSparkAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="CiscoSparkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables CiscoSpark authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the CiscoSpark options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddCiscoSpark(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<CiscoSparkAuthenticationOptions> configuration)
        {
            return builder.AddCiscoSpark(scheme, CiscoSparkAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="CiscoSparkAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables CiscoSpark authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the CiscoSpark options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddCiscoSpark(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<CiscoSparkAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<CiscoSparkAuthenticationOptions, CiscoSparkAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
