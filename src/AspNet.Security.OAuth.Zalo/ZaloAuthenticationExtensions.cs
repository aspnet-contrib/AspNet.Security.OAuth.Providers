using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.Zalo
{
    public static class ZaloAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddZalo(options => { });
        }

        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the Gitter options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<ZaloAuthenticationOptions> configuration)
        {
            return builder.AddZalo(
                ZaloAuthenticationDefaults.AuthenticationScheme,
                ZaloAuthenticationDefaults.DisplayName,
                configuration);
        }

        /// <summary>
        /// Adds <see cref="ZaloAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Zalo authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Gitter options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddZalo(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<ZaloAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<ZaloAuthenticationOptions, ZaloAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
