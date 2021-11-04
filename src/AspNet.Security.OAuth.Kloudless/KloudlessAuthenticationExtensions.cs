/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Kloudless;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Kloudless authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class KloudlessAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="KloudlessAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Kloudless authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddKloudless([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddKloudless(KloudlessAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="KloudlessAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Kloudless authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddKloudless(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<KloudlessAuthenticationOptions> configuration)
        {
            return builder.AddKloudless(KloudlessAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="KloudlessAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Kloudless authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Kloudless options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddKloudless(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<KloudlessAuthenticationOptions> configuration)
        {
            return builder.AddKloudless(scheme, KloudlessAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="KloudlessAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Kloudless authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Kloudless options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddKloudless(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<KloudlessAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<KloudlessAuthenticationOptions, KloudlessAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
