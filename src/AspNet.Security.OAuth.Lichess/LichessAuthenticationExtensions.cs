/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Lichess;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Lichess authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class LichessAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="LichessAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Lichess authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddLichess([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddLichess(LichessAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="LichessAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Lichess authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddLichess(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<LichessAuthenticationOptions> configuration)
    {
        return builder.AddLichess(LichessAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="LichessAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Lichess authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Lichess options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddLichess(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<LichessAuthenticationOptions> configuration)
    {
        return builder.AddLichess(scheme, LichessAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="LichessAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Lichess authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Lichess options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddLichess(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<LichessAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<LichessAuthenticationOptions, LichessAuthenticationHandler>(scheme, caption, configuration);
    }
}
