/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Twitter;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Twitter authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class TwitterAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="TwitterAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Twitter authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTwitter([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddTwitter(TwitterAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="TwitterAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Twitter authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Twitter options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTwitter(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<TwitterAuthenticationOptions> configuration)
    {
        return builder.AddTwitter(TwitterAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="TwitterAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Twitter authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Twitter options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTwitter(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<TwitterAuthenticationOptions> configuration)
    {
        return builder.AddTwitter(scheme, TwitterAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="TwitterAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Twitter authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Twitter options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTwitter(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<TwitterAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<TwitterAuthenticationOptions, TwitterAuthenticationHandler>(scheme, caption, configuration);
    }
}
