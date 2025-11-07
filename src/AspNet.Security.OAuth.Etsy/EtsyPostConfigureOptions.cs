/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Etsy;

/// <summary>
/// Applies Etsy-specific post-configuration logic after user configuration and base OAuth setup.
/// </summary>
public sealed class EtsyPostConfigureOptions : IPostConfigureOptions<EtsyAuthenticationOptions>
{
    public void PostConfigure(string? name, EtsyAuthenticationOptions options)
    {
        // Auto-add the email_r scope if detailed user info was requested but the scope not explicitly supplied.
        if (options.IncludeDetailedUserInfo && !options.Scope.Contains(EtsyAuthenticationConstants.Scopes.EmailRead))
        {
            options.Scope.Add(EtsyAuthenticationConstants.Scopes.EmailRead);

            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "primary_email");
            options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
        }

        // NOTE: We intentionally DO NOT auto-map the image to reduce data bloat,
        // as the image data can be quite large and is not always needed.
    }
}
