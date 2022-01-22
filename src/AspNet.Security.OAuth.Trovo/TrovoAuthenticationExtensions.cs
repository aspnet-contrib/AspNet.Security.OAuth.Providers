/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Trovo;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Trovo  authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class TrovoAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="TrovoAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trovo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrovo([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddTrovo(TrovoAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="TrovoAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trovo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrovo(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<TrovoAuthenticationOptions> configuration)
    {
        return builder.AddTrovo(TrovoAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="TrovoAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trovo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Trovo options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrovo(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<TrovoAuthenticationOptions> configuration)
    {
        return builder.AddTrovo(scheme, TrovoAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="TrovoAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Trovo authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Trovo options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTrovo(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<TrovoAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<TrovoAuthenticationOptions, TrovoAuthenticationHandler>(scheme, caption, configuration);
    }
}
