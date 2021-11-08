/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Zendesk
{
    /// <summary>
    /// Extension methods to add Zendesk authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class ZendeskAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="ZendeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zendesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddZendesk([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddZendesk(ZendeskAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="ZendeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zendesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddZendesk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<ZendeskAuthenticationOptions> configuration)
        {
            return builder.AddZendesk(ZendeskAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="ZendeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zendesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Zendesk options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZendesk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<ZendeskAuthenticationOptions> configuration)
        {
            return builder.AddZendesk(scheme, ZendeskAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="ZendeskAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zendesk authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Zendesk options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZendesk(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [CanBeNull] string caption,
            [NotNull] Action<ZendeskAuthenticationOptions> configuration)
        {
            builder.Services.TryAddSingleton<IPostConfigureOptions<ZendeskAuthenticationOptions>, ZendeskPostConfigureOptions>();
            return builder.AddOAuth<ZendeskAuthenticationOptions, ZendeskAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
