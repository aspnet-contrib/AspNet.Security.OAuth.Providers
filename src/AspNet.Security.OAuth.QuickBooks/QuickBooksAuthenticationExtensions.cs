﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.QuickBooks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add QuickBooks authentication capabilities to an HTTP application pipeline.
/// </summary>
public static class QuickBooksAuthenticationExtensions
{
    /// <summary>
    /// Adds <see cref="QuickBooksAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables QuickBooks authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddQuickBooks([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddQuickBooks(QuickBooksAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    /// <summary>
    /// Adds <see cref="QuickBooksAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables QuickBooks authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddQuickBooks(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<QuickBooksAuthenticationOptions> configuration)
    {
        return builder.AddQuickBooks(QuickBooksAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    /// <summary>
    /// Adds <see cref="QuickBooksAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables QuickBooks authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the QuickBooks options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddQuickBooks(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<QuickBooksAuthenticationOptions> configuration)
    {
        return builder.AddQuickBooks(scheme, QuickBooksAuthenticationDefaults.DisplayName, configuration);
    }

    /// <summary>
    /// Adds <see cref="QuickBooksAuthenticationHandler"/> to the specified
    /// <see cref="AuthenticationBuilder"/>, which enables QuickBooks authentication capabilities.
    /// </summary>
    /// <param name="builder">The authentication builder.</param>
    /// <param name="scheme">The authentication scheme associated with this instance.</param>
    /// <param name="caption">The optional display name associated with this instance.</param>
    /// <param name="configuration">The delegate used to configure the QuickBooks options.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddQuickBooks(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [CanBeNull] string caption,
        [NotNull] Action<QuickBooksAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<QuickBooksAuthenticationOptions, QuickBooksAuthenticationHandler>(scheme, caption, configuration);
    }
}
