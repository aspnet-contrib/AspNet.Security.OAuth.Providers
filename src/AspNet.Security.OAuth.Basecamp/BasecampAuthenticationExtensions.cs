/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Basecamp;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Basecamp authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class BasecampAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="BasecampAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Basecamp authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddBasecamp([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddBasecamp(BasecampAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="BasecampAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Basecamp authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddBasecamp(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<BasecampAuthenticationOptions> configuration)
        {
            return builder.AddBasecamp(BasecampAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="BasecampAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Basecamp authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Basecamp options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBasecamp(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<BasecampAuthenticationOptions> configuration)
        {
            return builder.AddBasecamp(scheme, BasecampAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="BasecampAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Basecamp authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Basecamp options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBasecamp(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<BasecampAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<BasecampAuthenticationOptions, BasecampAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
