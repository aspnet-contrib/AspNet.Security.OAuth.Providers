/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Etsy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Etsy authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class EtsyAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="EtsyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Etsy authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEtsy([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddEtsy(EtsyAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="EtsyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Etsy authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Etsy options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEtsy(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<EtsyAuthenticationOptions> configuration)
    {
        return builder.AddEtsy(EtsyAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="EtsyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Etsy authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Etsy options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEtsy(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<EtsyAuthenticationOptions> configuration)
    {
        return builder.AddEtsy(scheme, EtsyAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="EtsyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Etsy authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Etsy options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddEtsy(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<EtsyAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<EtsyAuthenticationOptions>, EtsyPostConfigureOptions>();
        return builder.AddOAuth<EtsyAuthenticationOptions, EtsyAuthenticationHandler>(scheme, caption, configuration);
    }
}
