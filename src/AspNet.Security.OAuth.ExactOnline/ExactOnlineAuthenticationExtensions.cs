/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.ExactOnline;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add ExactOnline authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class ExactOnlineAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="ExactOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ExactOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddExactOnline([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddExactOnline(ExactOnlineAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="ExactOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ExactOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddExactOnline(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<ExactOnlineAuthenticationOptions> configuration)
    {
        return builder.AddExactOnline(ExactOnlineAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="ExactOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ExactOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the ExactOnline options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddExactOnline(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<ExactOnlineAuthenticationOptions> configuration)
    {
        return builder.AddExactOnline(scheme, ExactOnlineAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="ExactOnlineAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ExactOnline authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the ExactOnline options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddExactOnline(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<ExactOnlineAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<ExactOnlineAuthenticationOptions, ExactOnlineAuthenticationHandler>(scheme, caption, configuration);
    }
}
