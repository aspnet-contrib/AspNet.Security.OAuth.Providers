/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Linear;

namespace Microsoft.Extensions.DependencyInjection;

public static class LinearAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="LinearAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Linear authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddLinear([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddLinear(LinearAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="LinearAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Linear authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Linear options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddLinear(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<LinearAuthenticationOptions> configuration)
    {
        return builder.AddLinear(LinearAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="LinearAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Linear authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Linear options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddLinear(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<LinearAuthenticationOptions> configuration)
    {
        return builder.AddLinear(scheme, LinearAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="LinearAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Linear authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Linear options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddLinear(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] string caption,
        [NotNull] Action<LinearAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<LinearAuthenticationOptions, LinearAuthenticationHandler>(scheme, caption, configuration);
    }
}
