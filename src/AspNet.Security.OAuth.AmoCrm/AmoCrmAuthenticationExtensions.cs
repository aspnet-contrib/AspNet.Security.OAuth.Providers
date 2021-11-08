/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.AmoCrm;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add amoCRM authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class AmoCrmAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="AmoCrmAuthenticationHandler"/> to the specified
    /// <see cref="ApplicationBuilder"/>, which enables amoCRM authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddAmoCrm([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddAmoCrm(AmoCrmAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="AmoCrmAuthenticationHandler"/> to the specified
    /// <see cref="ApplicationBuilder"/>, which enables amoCRM authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the amoCRM options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddAmoCrm(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<AmoCrmAuthenticationOptions> configuration)
    {
        return builder.AddAmoCrm(AmoCrmAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="AmoCrmAuthenticationHandler"/> to the specified
    /// <see cref="ApplicationBuilder"/>, which enables amoCRM authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the amoCRM options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddAmoCrm(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<AmoCrmAuthenticationOptions> configuration)
    {
        return builder.AddAmoCrm(scheme, AmoCrmAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="AmoCrmAuthenticationHandler"/> to the specified
    /// <see cref="ApplicationBuilder"/>, which enables amoCRM authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the amoCRM options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddAmoCrm(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] string caption,
        [NotNull] Action<AmoCrmAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<AmoCrmAuthenticationOptions, AmoCrmAuthenticationHandler>(scheme, caption, configuration);
    }
}
