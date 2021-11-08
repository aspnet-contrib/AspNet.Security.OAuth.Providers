/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.MailRu;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add MailRu authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class MailRuAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="MailRuAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables MailRu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMailRu([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddMailRu(MailRuAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="MailRuAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables MailRu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMailRu(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<MailRuAuthenticationOptions> configuration)
        {
            return builder.AddMailRu(MailRuAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="MailRuAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables MailRu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the MailRu options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMailRu(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<MailRuAuthenticationOptions> configuration)
        {
            return builder.AddMailRu(scheme, MailRuAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="MailRuAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables MailRu authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the MailRu options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddMailRu(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<MailRuAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<MailRuAuthenticationOptions, MailRuAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
