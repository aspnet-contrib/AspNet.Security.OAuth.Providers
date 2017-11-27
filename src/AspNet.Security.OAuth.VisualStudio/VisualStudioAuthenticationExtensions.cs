/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://VisualStudio.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.VisualStudio;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add  authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class VisualStudioAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="VisualStudioAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables VisualStudio authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVisualStudio([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddVisualStudio(VisualStudioAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="VisualStudioAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables VisualStudio authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVisualStudio(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<VisualStudioAuthenticationOptions> configuration)
        {
            return builder.AddVisualStudio(VisualStudioAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="VisualStudioAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables VisualStudio authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the VisualStudio options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVisualStudio(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<VisualStudioAuthenticationOptions> configuration)
        {
            return builder.AddVisualStudio(scheme, VisualStudioAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="VisualStudioAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables VisualStudio authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the VisualStudio options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddVisualStudio(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<VisualStudioAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<VisualStudioAuthenticationOptions, VisualStudioAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
