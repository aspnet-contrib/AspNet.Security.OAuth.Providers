/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.Typeform;

/// <summary>
/// Extension methods to add Typeform authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class TypeformAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="TypeformAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Typeform authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddTypeform([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddTypeform(TypeformAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="TypeformAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Typeform authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddTypeform(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<TypeformAuthenticationOptions> configuration)
    {
        return builder.AddTypeform(TypeformAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="TypeformAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Typeform authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Typeform options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTypeform(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<TypeformAuthenticationOptions> configuration)
    {
        return builder.AddTypeform(scheme, TypeformAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="TypeformAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Typeform authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Typeform options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddTypeform(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<TypeformAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<TypeformAuthenticationOptions, TypeformAuthenticationHandler>(scheme, caption, configuration);
    }
}
