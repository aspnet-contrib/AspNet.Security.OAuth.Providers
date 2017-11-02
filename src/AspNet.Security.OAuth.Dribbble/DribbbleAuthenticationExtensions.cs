/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNet.Security.OAuth.Dribbble
{
    /// <summary>
    /// Extension methods to add Dribbble authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class DribbbleAuthenticationExtensions
    {
        /// <summary>
        /// Adds the <see cref="DribbbleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Dribbble authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddDribbble(this AuthenticationBuilder builder)
        {
            return builder.AddDribbble(DribbbleAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds the <see cref="DribbbleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Dribbble authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddDribbble(
            this AuthenticationBuilder builder,
            Action<DribbbleAuthenticationOptions> configuration)
        {
            return builder.AddDribbble(DribbbleAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="DribbbleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Dribbble authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Dribbble options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDribbble(
            this AuthenticationBuilder builder, string scheme,
            Action<DribbbleAuthenticationOptions> configuration)
        {
            return builder.AddDribbble(scheme, DribbbleAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="DribbbleAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Dribbble authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="name">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Dribbble options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDribbble(
            this AuthenticationBuilder builder,
            string scheme, string name,
            Action<DribbbleAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<DribbbleAuthenticationOptions, DribbbleAuthenticationHandler>(scheme, name, configuration);
        }
    }
}
