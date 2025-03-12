// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.Atlassian;

/// <summary>
/// Extension methods to add Atlassian authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class AtlassianAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="AtlassianAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Atlassian authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddAtlassian([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddAtlassian(AtlassianAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="AtlassianAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Atlassian authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddAtlassian(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<AtlassianAuthenticationOptions> configuration)
    {
        return builder.AddAtlassian(AtlassianAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="AtlassianAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Atlassian authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Atlassian options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddAtlassian(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<AtlassianAuthenticationOptions> configuration)
    {
        return builder.AddAtlassian(scheme, AtlassianAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="AtlassianAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Atlassian authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Atlassian options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddAtlassian(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<AtlassianAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<AtlassianAuthenticationOptions, AtlassianAuthenticationHandler>(scheme, caption, configuration);
    }
}
