/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Onshape;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Onshape authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class OnshapeAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="OnshapeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Onshape authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOnshape([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddOnshape(OnshapeAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="OnshapeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Onshape authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOnshape(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration)
        {
            return builder.AddOnshape(OnshapeAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="OnshapeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Onshape authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Onshape options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOnshape(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration)
        {
            return builder.AddOnshape(scheme, OnshapeAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="OnshapeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Onshape authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Onshape options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddOnshape(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<OnshapeAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<OnshapeAuthenticationOptions, OnshapeAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
