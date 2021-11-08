/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Keycloak;

/// <summary>
/// Defines a set of options used by <see cref="KeycloakAuthenticationHandler"/>.
/// </summary>
public class KeycloakAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeycloakAuthenticationOptions"/> class.
    /// </summary>
    public KeycloakAuthenticationOptions()
    {
        ClaimsIssuer = KeycloakAuthenticationDefaults.Issuer;
        CallbackPath = KeycloakAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = KeycloakAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = KeycloakAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = KeycloakAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("openid");

        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
        ClaimActions.MapJsonKey(ClaimTypes.Role, "roles");
    }

    /// <summary>
    /// Gets or sets the value for the Keycloak client's access type.
    /// </summary>
    public KeycloakAuthenticationAccessType AccessType { get; set; }

    /// <summary>
    /// Gets or sets the base address of the Keycloak server.
    /// </summary>
    public Uri? BaseAddress { get; set; }

    /// <summary>
    /// Gets or sets the Keycloak domain (Org URL) to use for authentication.
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// Gets or sets the Keycloak realm to use for authentication.
    /// </summary>
    public string? Realm { get; set; }

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
        catch (ArgumentException ex) when (ex.ParamName == nameof(ClientSecret) && AccessType == KeycloakAuthenticationAccessType.Public)
        {
            // No client secret is required for a public key.
            // See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers/issues/610.
        }

        if (string.IsNullOrEmpty(AuthorizationEndpoint))
        {
            throw new ArgumentException($"The '{nameof(AuthorizationEndpoint)}' option must be provided.", nameof(AuthorizationEndpoint));
        }

        if (string.IsNullOrEmpty(TokenEndpoint))
        {
            throw new ArgumentException($"The '{nameof(TokenEndpoint)}' option must be provided.", nameof(TokenEndpoint));
        }

        if (!CallbackPath.HasValue)
        {
            throw new ArgumentException($"The '{nameof(CallbackPath)}' option must be provided.", nameof(CallbackPath));
        }
    }
}
