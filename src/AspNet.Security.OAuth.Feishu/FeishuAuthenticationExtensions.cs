/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Feishu;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Feishu authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class FeishuAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="FeishuAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Feishu authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFeishu([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddFeishu(FeishuAuthenticationDefaults.AuthenticationScheme, _ => { });
    }

    /// <summary>
    /// Adds <see cref="FeishuAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Feishu authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFeishu(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<FeishuAuthenticationOptions> configuration)
    {
        return builder.AddFeishu(FeishuAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="FeishuAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Feishu authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Feishu options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFeishu(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<FeishuAuthenticationOptions> configuration)
    {
        return builder.AddFeishu(scheme, FeishuAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="FeishuAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Feishu authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Feishu options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddFeishu(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<FeishuAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<FeishuAuthenticationOptions, FeishuAuthenticationHandler>(scheme, caption, configuration);
    }
}
