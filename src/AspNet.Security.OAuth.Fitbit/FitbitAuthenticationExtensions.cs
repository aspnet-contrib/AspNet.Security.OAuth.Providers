﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Fitbit;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Fitbit authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class FitbitAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="FitbitAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Fitbit authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFitbit([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddFitbit(FitbitAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="FitbitAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Fitbit authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFitbit(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<FitbitAuthenticationOptions> configuration)
    {
        return builder.AddFitbit(FitbitAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="FitbitAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Fitbit authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Fitbit options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFitbit(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<FitbitAuthenticationOptions> configuration)
    {
        return builder.AddFitbit(scheme, FitbitAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="FitbitAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Fitbit authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Fitbit options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFitbit(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<FitbitAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<FitbitAuthenticationOptions, FitbitAuthenticationHandler>(scheme, caption, configuration);
    }
}
