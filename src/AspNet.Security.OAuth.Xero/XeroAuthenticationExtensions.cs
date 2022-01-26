/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Xero;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Xero authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class XeroAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="XeroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Xero authentication capabilities.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add the middleware to.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddXero([NotNull] this AuthenticationBuilder builder)
        => builder.AddXero(XeroAuthenticationDefaults.AuthenticationScheme, _ => { });

    /// <summary>
    /// Adds <see cref="XeroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Xero authentication capabilities.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/> to add the middleware to.</param>
    /// <param name="configuration">The delegate used to configure the OAuth 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddXero(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<XeroAuthenticationOptions> configuration)
        => builder.AddXero(XeroAuthenticationDefaults.AuthenticationScheme, configuration);

    /// <summary>
    /// Adds <see cref="XeroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Xero authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="authenticationScheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Xero options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddXero(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string authenticationScheme,
        [NotNull] Action<XeroAuthenticationOptions> configuration)
        => builder.AddXero(authenticationScheme, XeroAuthenticationDefaults.DisplayName, configuration);

    /// <summary>
    /// Adds <see cref="XeroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Xero authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="authenticationScheme">The authentication scheme associated with this instance.</param>
    /// <param name="displayName">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Xero options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddXero(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string authenticationScheme,
        [NotNull] string displayName,
        [NotNull] Action<XeroAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<XeroAuthenticationOptions>, XeroAuthenticationPostConfigureOptions>();
        return builder.AddOAuth<XeroAuthenticationOptions, XeroAuthenticationHandler>(authenticationScheme, displayName, configuration);
    }
}
