/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.VkId;

/// <summary>
/// Extension methods to add VK ID authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class VkIdAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="VkIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables VK ID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddVkId([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddVkId(VkIdAuthenticationDefaults.AuthenticationScheme, _ => { });
    }

    /// <summary>
    /// Adds <see cref="VkIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables VK ID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the <see cref="VkIdAuthenticationOptions"/> options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddVkId(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<VkIdAuthenticationOptions> configuration)
    {
        return builder.AddVkId(VkIdAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="VkIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables VK ID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the <see cref="VkIdAuthenticationOptions"/> options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddVkId(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<VkIdAuthenticationOptions> configuration)
    {
        return builder.AddVkId(scheme, VkIdAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="VkIdAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables VK ID authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the <see cref="VkIdAuthenticationOptions"/> options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddVkId(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<VkIdAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<VkIdAuthenticationOptions, VkIdAuthenticationHandler>(scheme, caption, configuration);
    }
}
