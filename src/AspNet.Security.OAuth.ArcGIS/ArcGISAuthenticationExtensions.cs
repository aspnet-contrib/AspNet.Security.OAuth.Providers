/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.ArcGIS;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add ArcGIS authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class ArcGISAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="ArcGISAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables ArcGIS authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddArcGIS([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddArcGIS(ArcGISAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="ArcGISAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables ArcGIS authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddArcGIS(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<ArcGISAuthenticationOptions> configuration)
        {
            return builder.AddArcGIS(ArcGISAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="ArcGISAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables ArcGIS authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the ArcGIS options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddArcGIS(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<ArcGISAuthenticationOptions> configuration)
        {
            return builder.AddArcGIS(scheme, ArcGISAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="ArcGISAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables ArcGIS authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the ArcGIS options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddArcGIS(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<ArcGISAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<ArcGISAuthenticationOptions, ArcGISAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
