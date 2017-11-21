/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Yammer;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Yammer authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class YammerAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="YammerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Yammer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddYammer([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddYammer(YammerAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds the <see cref="YammerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Yammer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddYammer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<YammerAuthenticationOptions> configuration)
        {
            return builder.AddYammer(YammerAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="YammerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Yammer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Yammer options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddYammer(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<YammerAuthenticationOptions> configuration)
        {
            return builder.AddYammer(scheme, YammerAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="YammerAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Yammer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="name">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Yammer options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddYammer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string name,
            [NotNull] Action<YammerAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<YammerAuthenticationOptions, YammerAuthenticationHandler>(scheme, name, configuration);
        }
    }
}
