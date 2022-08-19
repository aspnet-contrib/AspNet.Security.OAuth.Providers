/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Snapchat;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Snapchat authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class SnapchatAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="SnapchatAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Snapchat authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddSnapchat([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddSnapchat(SnapchatAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="SnapchatAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Snapchat authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddSnapchat(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<SnapchatAuthenticationOptions> configuration)
    {
        return builder.AddSnapchat(SnapchatAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="SnapchatAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Snapchat authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Snapchat options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddSnapchat(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<SnapchatAuthenticationOptions> configuration)
    {
        return builder.AddSnapchat(scheme, SnapchatAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="SnapchatAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Snapchat authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Snapchat options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddSnapchat(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<SnapchatAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<SnapchatAuthenticationOptions, SnapchatAuthenticationHandler>(scheme, caption, configuration);
    }
}
