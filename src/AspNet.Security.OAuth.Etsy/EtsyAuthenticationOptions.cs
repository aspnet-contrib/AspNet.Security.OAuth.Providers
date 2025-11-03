/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Etsy.EtsyAuthenticationConstants;

namespace AspNet.Security.OAuth.Etsy;

/// <summary>
/// Defines a set of options used by <see cref="EtsyAuthenticationHandler"/>.
/// </summary>
public class EtsyAuthenticationOptions : OAuthOptions
{
    public EtsyAuthenticationOptions()
    {
        ClaimsIssuer = EtsyAuthenticationDefaults.Issuer;
        CallbackPath = EtsyAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = EtsyAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = EtsyAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = EtsyAuthenticationDefaults.UserInformationEndpoint;

        // Enable PKCE by default (required by Etsy)
        UsePkce = true;

        // Enable refresh token support
        SaveTokens = true;

        // Default scopes - Etsy requires at least one scope
        Scope.Add(Scopes.EmailRead);
        Scope.Add(Scopes.ShopsRead);

        // Map Etsy user fields to standard and custom claims
        // These mappings apply to the /v3/application/users/{user_id} endpoint response
        // Note: ClaimTypes.NameIdentifier, UserId, and ShopId are set programmatically
        // in the handler from the /v3/application/users/me endpoint
        ClaimActions.MapJsonKey(ClaimTypes.Email, "primary_email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
        ClaimActions.MapJsonKey(Claims.PrimaryEmail, "primary_email");
        ClaimActions.MapJsonKey(Claims.FirstName, "first_name");
        ClaimActions.MapJsonKey(Claims.LastName, "last_name");
        ClaimActions.MapJsonKey(Claims.ImageUrl, "image_url_75x75");
    }
}
