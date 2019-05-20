/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Myob;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Myob authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class MyobAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="MyobAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Myob authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMyob([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddMyob(MyobAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="MyobAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Myob authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMyob(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<MyobAuthenticationOptions> configuration)
        {
            return builder.AddMyob(MyobAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="MyobAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Myob authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Myob options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMyob(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<MyobAuthenticationOptions> configuration)
        {
            return builder.AddMyob(scheme, MyobAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="MyobAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Myob authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Myob options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMyob(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<MyobAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<MyobAuthenticationOptions, MyobAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
