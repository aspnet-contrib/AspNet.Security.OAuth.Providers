/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Miro;

namespace Microsoft.Extensions.DependencyInjection;

public static class MiroAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="MiroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Miro authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddMiro([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddMiro(MiroAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="MiroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Miro authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Miro authentication options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddMiro(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<MiroAuthenticationOptions> configuration)
    {
        return builder.AddMiro(MiroAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="AspNet.Security.OAuth.Miro.MiroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Miro authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Miro authentication options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddMiro(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<MiroAuthenticationOptions> configuration)
    {
        return builder.AddMiro(scheme, MiroAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="AspNet.Security.OAuth.Miro.MiroAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Miro authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Miro authentication options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddMiro(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] string caption,
        [NotNull] Action<MiroAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<MiroAuthenticationOptions, MiroAuthenticationHandler>(scheme, caption, configuration);
    }
}
