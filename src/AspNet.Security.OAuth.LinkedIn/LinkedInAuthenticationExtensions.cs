/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.LinkedIn;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add LinkedIn authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class LinkedInAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="LinkedInAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables LinkedIn authentication capabilities.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddLinkedIn([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddLinkedIn(LinkedInAuthenticationDefaults.AuthenticationScheme, options => {});
        }

        /// <summary>
        /// Adds <see cref="LinkedInAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables LinkedIn authentication capabilities.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add the middleware to.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddLinkedIn(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return builder.AddLinkedIn(LinkedInAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="LinkedInAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables LinkedIn authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the LinkedIn options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddLinkedIn(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return builder.AddLinkedIn(scheme, LinkedInAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="LinkedInAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables LinkedIn authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the LinkedIn options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddLinkedIn(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<LinkedInAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<LinkedInAuthenticationOptions, LinkedInAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
