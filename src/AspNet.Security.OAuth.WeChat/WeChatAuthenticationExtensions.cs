/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using AspNet.Security.OAuth.WeChat;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add WeChat authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class WeChatAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="WeChatAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WeChat authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddWeChat([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddWeChat(WeChatAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="WeChatAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WeChat authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static AuthenticationBuilder AddWeChat(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<WeChatAuthenticationOptions> configuration)
        {
            return builder.AddWeChat(WeChatAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="WeChatAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WeChat authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the WeChat options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddWeChat(
            [NotNull] this AuthenticationBuilder builder, [NotNull] string scheme,
            [NotNull] Action<WeChatAuthenticationOptions> configuration)
        {
            return builder.AddWeChat(scheme, WeChatAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="WeChatAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables WeChat authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the WeChat options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddWeChat(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme, [CanBeNull] string caption,
            [NotNull] Action<WeChatAuthenticationOptions> configuration)
        {
            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<WeChatAuthenticationOptions>,
                    WeChatAuthenticationInitializer>());

            return builder.AddOAuth<WeChatAuthenticationOptions, WeChatAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}
