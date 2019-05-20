/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Autodesk;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Autodesk authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AutodeskAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="AutodeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Autodesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAutodesk([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddAutodesk(AutodeskAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="AutodeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Autodesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAutodesk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<AutodeskAuthenticationOptions> configuration)
        {
            return builder.AddAutodesk(AutodeskAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AutodeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Autodesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Autodesk options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAutodesk(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<AutodeskAuthenticationOptions> configuration)
        {
            return builder.AddAutodesk(scheme, AutodeskAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AutodeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Autodesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Autodesk options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAutodesk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<AutodeskAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AutodeskAuthenticationOptions, AutodeskAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
