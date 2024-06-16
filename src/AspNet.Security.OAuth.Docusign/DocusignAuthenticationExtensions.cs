/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Docusign;

/// <summary>
/// Extension methods to add Docusign authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class DocusignAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="DocusignAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Docusign authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddDocusign([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddDocusign(DocusignAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="DocusignAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Docusign authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddDocusign(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<DocusignAuthenticationOptions> configuration)
    {
        return builder.AddDocusign(DocusignAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="DocusignAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Docusign authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Docusign options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddDocusign(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<DocusignAuthenticationOptions> configuration)
    {
        return builder.AddDocusign(scheme, DocusignAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="DocusignAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Docusign authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Docusign options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddDocusign(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<DocusignAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<DocusignAuthenticationOptions>, DocusignAuthenticationPostConfigureOptions>();
        return builder.AddOAuth<DocusignAuthenticationOptions, DocusignAuthenticationHandler>(scheme, caption, configuration);
    }
}
