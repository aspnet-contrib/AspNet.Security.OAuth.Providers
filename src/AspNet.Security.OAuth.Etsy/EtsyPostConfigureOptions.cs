/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Etsy;

/// <summary>
/// Contains the methods required to ensure that the Etsy configuration is valid.
/// </summary>
public class EtsyPostConfigureOptions : IPostConfigureOptions<EtsyAuthenticationOptions>
{
    /// <summary>
    /// Invoked to post-configure a TOptions instance.
    /// </summary>
    /// <param name="name">The name of the options instance being configured.</param>
    /// <param name="options">The options instance to configure.</param>
    public void PostConfigure(string? name, EtsyAuthenticationOptions options)
    {
        if (string.IsNullOrEmpty(options.ClientId))
        {
            throw new ArgumentException("The Etsy Client ID cannot be null or empty.", nameof(options));
        }

        // Note: Client Secret validation removed - Etsy uses mandatory PKCE which provides
        // cryptographic proof of authorization code ownership, potentially eliminating the
        // need for client_secret in the token exchange. The ClientId (keystring) is used
        // in the x-api-key header for API authentication.

        // Ensure PKCE is enabled (required by Etsy)
        if (!options.UsePkce)
        {
            throw new ArgumentException("PKCE is required by Etsy and cannot be disabled.", nameof(options));
        }

        // Ensure at least one scope is requested (required by Etsy)
        if (options.Scope.Count == 0)
        {
            throw new ArgumentException("At least one scope must be specified for Etsy authentication.", nameof(options));
        }
    }
}
