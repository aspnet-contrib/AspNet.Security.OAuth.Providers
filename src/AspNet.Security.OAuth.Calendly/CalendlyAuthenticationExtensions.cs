/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.OAuth.Calendly;

/// <summary>
/// Extension methods to add Calendly authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class CalendlyAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="CalendlyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Calendly authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddCalendly([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddCalendly(CalendlyAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="CalendlyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Calendly authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddCalendly(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<CalendlyAuthenticationOptions> configuration)
    {
        return builder.AddCalendly(CalendlyAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="CalendlyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Calendly authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Calendly options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddCalendly(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<CalendlyAuthenticationOptions> configuration)
    {
        return builder.AddCalendly(scheme, CalendlyAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="CalendlyAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Calendly authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Calendly options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddCalendly(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<CalendlyAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<CalendlyAuthenticationOptions, CalendlyAuthenticationHandler>(scheme, caption, configuration);
    }
}
