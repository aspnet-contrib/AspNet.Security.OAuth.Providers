/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.BungieNet;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add  authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class BungieNetAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="BungieNetAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables BungieNet authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddBungieNet([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddBungieNet(BungieNetAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="BungieNetAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables BungieNet authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddBungieNet(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<BungieNetAuthenticationOptions> configuration)
    {
        return builder.AddBungieNet(BungieNetAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="BungieNetAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables BungieNet authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the BungieNet options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddBungieNet(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<BungieNetAuthenticationOptions> configuration)
    {
        return builder.AddBungieNet(scheme, BungieNetAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="BungieNetAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables BungieNet authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the BungieNet options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddBungieNet(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<BungieNetAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<BungieNetAuthenticationOptions, BungieNetAuthenticationHandler>(scheme, caption, configuration);
    }
}
