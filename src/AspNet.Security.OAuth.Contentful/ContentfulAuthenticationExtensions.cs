/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.Contentful;

namespace Microsoft.Extensions.DependencyInjection;

public static class ContentfulAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="ContentfulAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Contentful authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddContentful([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddContentful(ContentfulAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="ContentfulAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Contentful authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the Contentful options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static AuthenticationBuilder AddContentful(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<ContentfulAuthenticationOptions> configuration)
    {
        return builder.AddContentful(ContentfulAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="ContentfulAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Contentful authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Contentful options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddContentful(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<ContentfulAuthenticationOptions> configuration)
    {
        return builder.AddContentful(scheme, ContentfulAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="ContentfulAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables Contentful authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the Contentful options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddContentful(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] string caption,
        [NotNull] Action<ContentfulAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<ContentfulAuthenticationOptions, ContentfulAuthenticationHandler>(scheme, caption, configuration);
    }
}
