/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Buffer;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Buffer authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class BufferAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="BufferAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Buffer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBuffer([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddBuffer(BufferAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="BufferAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Buffer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBuffer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<BufferAuthenticationOptions> configuration)
        {
            return builder.AddBuffer(BufferAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="BufferAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Buffer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Buffer options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBuffer(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<BufferAuthenticationOptions> configuration)
        {
            return builder.AddBuffer(scheme, BufferAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="BufferAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Buffer authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Buffer options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBuffer(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<BufferAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<BufferAuthenticationOptions, BufferAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
