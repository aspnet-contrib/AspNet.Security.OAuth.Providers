/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Asana;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Asana authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class AsanaAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="AsanaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAsana([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddAsana(AsanaAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds the <see cref="AsanaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddAsana(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<AsanaAuthenticationOptions> configuration)
        {
            return builder.AddAsana(AsanaAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="AsanaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Asana options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAsana(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<AsanaAuthenticationOptions> configuration)
        {
            return builder.AddAsana(scheme, AsanaAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="AsanaAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Asana authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="name">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Asana options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddAsana(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string name,
            [NotNull] Action<AsanaAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<AsanaAuthenticationOptions, AsanaAuthenticationHandler>(scheme, name, configuration);
        }
    }
}
