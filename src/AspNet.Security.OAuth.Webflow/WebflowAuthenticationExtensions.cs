/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Webflow;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Webflow authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class WebflowAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="WebflowAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Webflow authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddWebflow([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddWebflow(WebflowAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="WebflowAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Webflow authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Webflow authentication options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddWebflow(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<WebflowAuthenticationOptions> configuration)
    {
        return builder.AddWebflow(WebflowAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="WebflowAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Webflow authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Webflow authentication options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWebflow(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<WebflowAuthenticationOptions> configuration)
    {
        return builder.AddWebflow(scheme, WebflowAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="WebflowAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Webflow authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Webflow authentication options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddWebflow(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] string caption,
        [NotNull] Action<WebflowAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<WebflowAuthenticationOptions, WebflowAuthenticationHandler>(scheme, caption, configuration);
    }
}
