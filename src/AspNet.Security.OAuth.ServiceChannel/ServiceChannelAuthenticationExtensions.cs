/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.ServiceChannel;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add ServiceChannel authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class ServiceChannelAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="ServiceChannelAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ServiceChannel authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddServiceChannel([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddServiceChannel(ServiceChannelAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="ServiceChannelAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ServiceChannel authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddServiceChannel(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<ServiceChannelAuthenticationOptions> configuration)
    {
        return builder.AddServiceChannel(ServiceChannelAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="ServiceChannelAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ServiceChannel authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the ServiceChannel options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddServiceChannel(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<ServiceChannelAuthenticationOptions> configuration)
    {
        return builder.AddServiceChannel(scheme, ServiceChannelAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="ServiceChannelAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables ServiceChannel authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the ServiceChannel options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddServiceChannel(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<ServiceChannelAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<ServiceChannelAuthenticationOptions, ServiceChannelAuthenticationHandler>(scheme, caption, configuration);
    }
}
