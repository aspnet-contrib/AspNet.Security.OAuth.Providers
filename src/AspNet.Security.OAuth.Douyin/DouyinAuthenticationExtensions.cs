/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Douyin;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Douyin authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class DouyinAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="DouyinAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Douyin authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddDouyin([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddDouyin(DouyinAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="DouyinAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Douyin authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddDouyin(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<DouyinAuthenticationOptions> configuration)
    {
        return builder.AddDouyin(DouyinAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="DouyinAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Douyin authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Douyin options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddDouyin(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<DouyinAuthenticationOptions> configuration)
    {
        return builder.AddDouyin(scheme, DouyinAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="DouyinAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Douyin authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Douyin options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddDouyin(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<DouyinAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<DouyinAuthenticationOptions, DouyinAuthenticationHandler>(scheme, caption, configuration);
    }
}
