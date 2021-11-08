/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Harvest;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Harvest authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class HarvestAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="HarvestAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Harvest authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHarvest([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddHarvest(HarvestAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="HarvestAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Harvest authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHarvest(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<HarvestAuthenticationOptions> configuration)
    {
        return builder.AddHarvest(HarvestAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="HarvestAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Harvest authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Harvest options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHarvest(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<HarvestAuthenticationOptions> configuration)
    {
        return builder.AddHarvest(scheme, HarvestAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="HarvestAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Harvest authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Harvest options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHarvest(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<HarvestAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<HarvestAuthenticationOptions, HarvestAuthenticationHandler>(scheme, caption, configuration);
    }
}
