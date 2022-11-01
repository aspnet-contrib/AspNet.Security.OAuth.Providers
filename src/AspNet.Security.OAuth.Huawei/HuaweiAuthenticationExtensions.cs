/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Huawei;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Huawei authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class HuaweiAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="HuaweiAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Huawei authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHuawei([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddHuawei(HuaweiAuthenticationDefaults.AuthenticationScheme, _ => { });
    }

    /// <summary>
    /// Adds <see cref="HuaweiAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Huawei authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHuawei(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<HuaweiAuthenticationOptions> configuration)
    {
        return builder.AddHuawei(HuaweiAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="HuaweiAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Huawei authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Huawei options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHuawei(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<HuaweiAuthenticationOptions> configuration)
    {
        return builder.AddHuawei(scheme, HuaweiAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="HuaweiAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Huawei authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Huawei options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddHuawei(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<HuaweiAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<HuaweiAuthenticationOptions, HuaweiAuthenticationHandler>(scheme, caption, configuration);
    }
}
