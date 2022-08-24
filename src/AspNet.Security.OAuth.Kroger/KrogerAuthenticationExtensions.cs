/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Kroger;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add Kroger authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class KrogerAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="KrogerAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kroger authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddKroger([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddKroger(KrogerAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="KrogerAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kroger authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Kroger options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddKroger(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<KrogerAuthenticationOptions> configuration)
    {
        return builder.AddKroger(KrogerAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="KrogerAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kroger authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Kroger options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddKroger(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<KrogerAuthenticationOptions> configuration)
    {
        return builder.AddKroger(scheme, KrogerAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="KrogerAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Kroger authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Kroger options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddKroger(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<KrogerAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<KrogerAuthenticationOptions, KrogerAuthenticationHandler>(scheme, caption, configuration);
    }
}
