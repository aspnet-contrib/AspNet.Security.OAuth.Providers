/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.Bitbucket;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Bitbucket authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class BitbucketAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="BitbucketAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitbucket authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitbucket([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddBitbucket(BitbucketAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="BitbucketAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitbucket authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitbucket(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<BitbucketAuthenticationOptions> configuration)
        {
            return builder.AddBitbucket(BitbucketAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="BitbucketAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitbucket authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Bitbucket options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitbucket(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<BitbucketAuthenticationOptions> configuration)
        {
            return builder.AddBitbucket(scheme, BitbucketAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="BitbucketAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Bitbucket authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Bitbucket options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddBitbucket(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<BitbucketAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<BitbucketAuthenticationOptions, BitbucketAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
