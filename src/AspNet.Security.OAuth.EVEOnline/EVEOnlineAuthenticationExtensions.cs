﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.EVEOnline;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add EVEOnline authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class EVEOnlineAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="EVEOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables EVEOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEVEOnline([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddEVEOnline(EVEOnlineAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="EVEOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables EVEOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEVEOnline(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<EVEOnlineAuthenticationOptions> configuration)
    {
        return builder.AddEVEOnline(EVEOnlineAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="EVEOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables EVEOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the EVEOnline options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEVEOnline(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<EVEOnlineAuthenticationOptions> configuration)
    {
        return builder.AddEVEOnline(scheme, EVEOnlineAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="EVEOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables EVEOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the EVEOnline options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEVEOnline(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<EVEOnlineAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<EVEOnlineAuthenticationOptions>, EVEOnlinePostConfigureOptions>();

        return builder.AddOAuth<EVEOnlineAuthenticationOptions, EVEOnlineAuthenticationHandler>(scheme, caption, configuration);
    }
}
