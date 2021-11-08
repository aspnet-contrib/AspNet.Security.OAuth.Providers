/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.Mixcloud;

/// <summary>
/// Extension methods to add Mixcloud authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class MixcloudAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="MixcloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Mixcloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddMixcloud([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddMixcloud(MixcloudAuthenticationDefaults.AuthenticationScheme, _ => { });
    }

    /// <summary>
    /// Adds <see cref="MixcloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Mixcloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddMixcloud(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<MixcloudAuthenticationOptions> configuration)
    {
        return builder.AddMixcloud(MixcloudAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="MixcloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Mixcloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Mixcloud options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddMixcloud(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<MixcloudAuthenticationOptions> configuration)
    {
        return builder.AddMixcloud(scheme, MixcloudAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="MixcloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Mixcloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Mixcloud options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddMixcloud(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<MixcloudAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<MixcloudAuthenticationOptions, MixcloudAuthenticationHandler>(scheme, caption, configuration);
    }
}
