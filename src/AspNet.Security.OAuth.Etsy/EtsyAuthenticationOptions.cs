/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.Extensions.Options;
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

        UsePkce = true;
        SaveTokens = true;

        // Etsy requires at least one scope and this is the one for basic user info
        Scope.Add(Scopes.ShopsRead);

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
        ClaimActions.MapJsonKey(Claims.ShopId, "shop_id");
    }

    /// <summary>
    /// Gets or sets a value indicating whether to fetch detailed user information
    /// from the <see href="https://developers.etsy.com/documentation/reference#operation/getUser">getUser</see> Endpoint.
    /// </summary>
    public bool IncludeDetailedUserInfo { get; set; }

    /// <inheritdoc />
    public override void Validate()
    {
        try
        {
            // HACK We want all of the base validation except for ClientSecret,
            // so rather than re-implement it all, catch the exception thrown
            // for that being null and only throw if we aren't using public access type.
            // This does mean that three checks have to be re-implemented
            // because the won't be validated if the ClientSecret validation fails.
            base.Validate();
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(ClientSecret))
        {
            // No client secret is required for Etsy API, which uses Authorization Code Flow https://datatracker.ietf.org/doc/html/rfc6749#section-1.3.1 with:
            // See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/610.
        }

        // Ensure PKCE is enabled (required by Etsy)
        if (!UsePkce)
        {
            throw new ArgumentException("PKCE is required by Etsy Authentication and must be enabled.", nameof(UsePkce));
        }

        if (!SaveTokens)
        {
            throw new ArgumentException("Saving tokens is required by Etsy Authentication and must be enabled.", nameof(SaveTokens));
        }

        if (string.IsNullOrEmpty(AuthorizationEndpoint))
        {
            throw new ArgumentNullException($"The '{nameof(AuthorizationEndpoint)}' option must be provided.", nameof(AuthorizationEndpoint));
        }

        if (string.IsNullOrEmpty(TokenEndpoint))
        {
            throw new ArgumentNullException($"The '{nameof(TokenEndpoint)}' option must be provided.", nameof(TokenEndpoint));
        }

        if (string.IsNullOrEmpty(UserInformationEndpoint))
        {
            throw new ArgumentNullException($"The '{nameof(UserInformationEndpoint)}' option must be provided.", nameof(UserInformationEndpoint));
        }

        // Ensure at least one scope is requested (required by Etsy)
        if (Scope.Count == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(Scope), string.Join(',', Scope), "At least one scope must be specified for Etsy authentication.");
        }

        if (!Scope.Contains(Scopes.ShopsRead))
        {
            // ShopsRead scope is required to access basic user info
            throw new ArgumentOutOfRangeException(nameof(Scope), string.Join(',', Scope), $"The '{Scopes.ShopsRead}' scope must be specified for Etsy authentication UserInfoEndpoint: https://developers.etsy.com/documentation/reference#operation/getMe");
        }

        if (IncludeDetailedUserInfo && !Scope.Contains(Scopes.EmailRead))
        {
            // EmailRead scope is required to access detailed user info
            throw new ArgumentOutOfRangeException(nameof(Scope), string.Join(',', Scope), $"The '{Scopes.EmailRead}' scope must be specified for Etsy authentication when '{nameof(IncludeDetailedUserInfo)}' is enabled.");
        }

        if (!CallbackPath.HasValue)
        {
            throw new ArgumentException($"The '{nameof(CallbackPath)}' option must be provided.", nameof(CallbackPath));
        }
    }
}
