﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Patreon;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Patreon authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class PatreonAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="PatreonAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Patreon authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPatreon([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddPatreon(PatreonAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="PatreonAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Patreon authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPatreon(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<PatreonAuthenticationOptions> configuration)
    {
        return builder.AddPatreon(PatreonAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="PatreonAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Patreon authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Patreon options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPatreon(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<PatreonAuthenticationOptions> configuration)
    {
        return builder.AddPatreon(scheme, PatreonAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="PatreonAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Patreon authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Patreon options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddPatreon(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<PatreonAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<PatreonAuthenticationOptions, PatreonAuthenticationHandler>(scheme, caption, configuration);
    }
}
