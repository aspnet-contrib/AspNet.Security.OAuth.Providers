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

        UsePkce = true;
        SaveTokens = true;

        // Etsy requires at least one scope and this is the one for basic user info
        Scope.Add(Scopes.ShopsRead);

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "user_id");
        ClaimActions.MapJsonKey(Claims.UserId, "user_id");
        ClaimActions.MapJsonKey(Claims.ShopId, "shop_id");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "primary_email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
    }

    /// <summary>
    /// Gets or sets a value indicating whether to fetch detailed user information
    /// from the <see href="https://developers.etsy.com/documentation/reference#operation/getUser">getUser</see> Endpoint.
    /// </summary>
    public bool IncludeDetailedUserInfo { get; set; }

    /// <summary>
    /// Gets or sets the endpoint used to retrieve detailed user information.
    /// </summary>
    public string DetailedUserInfoEndpoint { get; set; } = EtsyAuthenticationDefaults.DetailedUserInfoEndpoint;

    /// <inheritdoc />
    public override void Validate()
    {
        if (IncludeDetailedUserInfo && !Scope.Contains(Scopes.EmailRead))
        {
            Scope.Add(Scopes.EmailRead);
        }

        try
        {
            // HACK We want all of the base validation except for ClientSecret,
            // so rather than re-implement it all, catch the exception thrown
            // for that being null and only throw if we aren't using public client access type + PKCE.
            // Etsy's OAuth implementation does not require a client secret referring to the Documentation using PKCE (Proof Key for Code Exchange).
            // This does mean that three checks have to be re-implemented
            // because they won't be validated if the ClientSecret validation fails.
            base.Validate();
        }
        catch (ArgumentException ex) when (ex.ParamName == nameof(ClientSecret))
        {
            // No client secret is required for Etsy API, which uses Authorization Code Flow https://datatracker.ietf.org/doc/html/rfc6749#section-1.3.1 with:
            // See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/610.
        }

        if (string.IsNullOrEmpty(AuthorizationEndpoint))
        {
            throw new ArgumentNullException(nameof(AuthorizationEndpoint), $"The '{nameof(AuthorizationEndpoint)}' option must be provided.");
        }

        if (string.IsNullOrEmpty(TokenEndpoint))
        {
            throw new ArgumentNullException(nameof(TokenEndpoint), $"The '{nameof(TokenEndpoint)}' option must be provided.");
        }

        if (string.IsNullOrEmpty(UserInformationEndpoint))
        {
            throw new ArgumentNullException(nameof(UserInformationEndpoint), $"The '{nameof(UserInformationEndpoint)}' option must be provided.");
        }

        if (!CallbackPath.HasValue)
        {
            throw new ArgumentNullException(nameof(CallbackPath), $"The '{nameof(CallbackPath)}' option must be provided.");
        }
    }
}
