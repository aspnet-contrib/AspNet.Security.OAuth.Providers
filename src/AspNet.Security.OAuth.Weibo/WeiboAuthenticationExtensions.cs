﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Weibo;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Weibo authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class WeiboAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="WeiboAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Weibo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddWeibo([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddWeibo(WeiboAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="WeiboAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Weibo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddWeibo(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<WeiboAuthenticationOptions> configuration)
    {
        return builder.AddWeibo(WeiboAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="WeiboAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Weibo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Weibo options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWeibo(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<WeiboAuthenticationOptions> configuration)
    {
        return builder.AddWeibo(scheme, WeiboAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="WeiboAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Weibo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Weibo options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWeibo(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<WeiboAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<WeiboAuthenticationOptions, WeiboAuthenticationHandler>(scheme, caption, configuration);
    }
}
