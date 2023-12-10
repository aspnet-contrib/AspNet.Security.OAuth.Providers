/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.WorldID;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add WorldID authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class WorldIdAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="WorldIDAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables World ID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldID([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddWorldID(WorldIDAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="WorldIDAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables WorldID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldID(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<WorldIDAuthenticationOptions> configuration)
    {
        return builder.AddWorldID(WorldIDAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="WorldIDAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables WorldID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the WorldID options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldID(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<WorldIDAuthenticationOptions> configuration)
    {
        return builder.AddWorldID(scheme, WorldIDAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="WorldIDAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables WorldID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the WorldID options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWorldID(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<WorldIDAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<WorldIDAuthenticationOptions, WorldIDAuthenticationHandler>(scheme, caption, configuration);
    }
}
