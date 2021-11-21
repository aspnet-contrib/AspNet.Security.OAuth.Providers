/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Trakt;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Trakt authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class TraktAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="TraktAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trakt authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrakt([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddTrakt(TraktAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="TraktAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trakt authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrakt(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<TraktAuthenticationOptions> configuration)
    {
        return builder.AddTrakt(TraktAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="TraktAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trakt authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Trakt options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrakt(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<TraktAuthenticationOptions> configuration)
    {
        return builder.AddTrakt(scheme, TraktAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="TraktAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trakt authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Trakt options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrakt(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<TraktAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<TraktAuthenticationOptions, TraktAuthenticationHandler>(scheme, caption, configuration);
    }
}
