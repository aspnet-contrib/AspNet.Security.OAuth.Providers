/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.PingOne;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add PingOne authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class PingOneAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="PingOneAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables PingOne authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPingOne([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddPingOne(PingOneAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="PingOneAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables PingOne authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the PingOne options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPingOne(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<PingOneAuthenticationOptions> configuration)
    {
        return builder.AddPingOne(PingOneAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="PingOneAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables PingOne authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the PingOne options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPingOne(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<PingOneAuthenticationOptions> configuration)
    {
        return builder.AddPingOne(scheme, PingOneAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="PingOneAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables PingOne authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the PingOne options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPingOne(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<PingOneAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<PingOneAuthenticationOptions>, PingOnePostConfigureOptions>();
        return builder.AddOAuth<PingOneAuthenticationOptions, PingOneAuthenticationHandler>(scheme, caption, configuration);
    }
}
