/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Coinbase;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Coinbase authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class CoinbaseAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="CoinbaseAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Coinbase authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddCoinbase([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddCoinbase(CoinbaseAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="CoinbaseAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Coinbase authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddCoinbase(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<CoinbaseAuthenticationOptions> configuration)
    {
        return builder.AddCoinbase(CoinbaseAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="CoinbaseAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Coinbase authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Coinbase options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddCoinbase(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<CoinbaseAuthenticationOptions> configuration)
    {
        return builder.AddCoinbase(scheme, CoinbaseAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="CoinbaseAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Coinbase authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Coinbase options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddCoinbase(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<CoinbaseAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<CoinbaseAuthenticationOptions, CoinbaseAuthenticationHandler>(scheme, caption, configuration);
    }
}
