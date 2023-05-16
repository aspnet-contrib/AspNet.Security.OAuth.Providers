/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Kook;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Kook authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class KookAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="KookAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kook authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddKook([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddKook(KookAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="KookAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kook authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddKook(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<KookAuthenticationOptions> configuration)
    {
        return builder.AddKook(KookAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="KookAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kook authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Kook options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddKook(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<KookAuthenticationOptions> configuration)
    {
        return builder.AddKook(scheme, KookAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="KookAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kook authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Kook options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddKook(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<KookAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<KookAuthenticationOptions, KookAuthenticationHandler>(scheme, caption, configuration);
    }
}
