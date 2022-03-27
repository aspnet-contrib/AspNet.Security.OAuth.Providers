/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Naver;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Naver authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class NaverAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="NaverAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Naver authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddNaver([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddNaver(NaverAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="NaverAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Naver authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddNaver(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<NaverAuthenticationOptions> configuration)
    {
        return builder.AddNaver(NaverAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="NaverAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Naver authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Naver options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddNaver(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<NaverAuthenticationOptions> configuration)
    {
        return builder.AddNaver(scheme, NaverAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="NaverAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Naver authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Naver options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddNaver(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<NaverAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<NaverAuthenticationOptions, NaverAuthenticationHandler>(scheme, caption, configuration);
    }
}
