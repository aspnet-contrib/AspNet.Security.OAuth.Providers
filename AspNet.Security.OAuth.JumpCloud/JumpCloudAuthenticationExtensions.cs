/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.JumpCloud;

/// <summary>
/// Extension methods to add JumpCloud authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class JumpCloudAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="AspNet.Security.OAuth.JumpCloud.JumpCloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables JumpCloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddJumpCloud([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddJumpCloud(JumpCloudAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="AspNet.Security.OAuth.JumpCloud.JumpCloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables JumpCloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the JumpCloud options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddJumpCloud(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<JumpCloudAuthenticationOptions> configuration)
    {
        return builder.AddJumpCloud(JumpCloudAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="AspNet.Security.OAuth.JumpCloud.JumpCloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables JumpCloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the JumpCloud options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddJumpCloud(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<JumpCloudAuthenticationOptions> configuration)
    {
        return builder.AddJumpCloud(scheme, JumpCloudAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="AspNet.Security.OAuth.JumpCloud.JumpCloudAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables JumpCloud authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the JumpCloud options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddJumpCloud(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
         string? caption,
        [NotNull] Action<JumpCloudAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<JumpCloudAuthenticationOptions>, JumpCloudPostConfigureOptions>();
        return builder.AddOAuth<JumpCloudAuthenticationOptions, AspNet.Security.OAuth.JumpCloud.JumpCloudAuthenticationHandler>(scheme, caption, configuration);
    }
}
