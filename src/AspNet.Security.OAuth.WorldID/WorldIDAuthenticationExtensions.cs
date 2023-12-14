/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.WorldId;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add WorldId authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class WorldIdAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="WorldIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables World ID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldId([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddWorldId(WorldIdAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="WorldIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables WorldId authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldId(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<WorldIdAuthenticationOptions> configuration)
    {
        return builder.AddWorldId(WorldIdAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="WorldIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables WorldId authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the WorldId options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldId(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<WorldIdAuthenticationOptions> configuration)
    {
        return builder.AddWorldId(scheme, WorldIdAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="WorldIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables WorldId authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the WorldId options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldId(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<WorldIdAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<WorldIdAuthenticationOptions, WorldIdAuthenticationHandler>(scheme, caption, configuration);
    }
}
