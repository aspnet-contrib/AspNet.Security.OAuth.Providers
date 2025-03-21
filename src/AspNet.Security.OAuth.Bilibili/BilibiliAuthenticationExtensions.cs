/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Bilibili;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Bilibili authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class BilibiliAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="BilibiliAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Bilibili authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddBilibili([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddBilibili(BilibiliAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="BilibiliAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Bilibili authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddBilibili(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<BilibiliAuthenticationOptions> configuration)
    {
        return builder.AddBilibili(BilibiliAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="BilibiliAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Bilibili authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Bilibili options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddBilibili(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<BilibiliAuthenticationOptions> configuration)
    {
        return builder.AddBilibili(scheme, BilibiliAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="BilibiliAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Bilibili authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Bilibili options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddBilibili(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<BilibiliAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<BilibiliAuthenticationOptions, BilibiliAuthenticationHandler>(scheme, caption, configuration);
    }
}
