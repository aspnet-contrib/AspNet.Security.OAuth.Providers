/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.HubSpot;

/// <summary>
/// Default values used by the HubSpot authentication middleware.
/// </summary>
public static class HubSpotAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "HubSpot";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "HubSpot";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "HubSpot";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-hubspot";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://app.hubspot.com/oauth/authorize";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpoint = "	https://api.hubapi.com/oauth/v1/token";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// Get Information for OAuth 2.0 access or refresh token:
    /// Get the meta data for an access or refresh token.This
    /// can be used to get the email address of the HubSpot user
    /// that the token was created for, as well as the Hub ID that the token is associated with.
    /// https://developers.hubspot.com/docs/api/oauth/tokens
    /// </summary>
    public static readonly string UserInformationEndpointFormat = "https://api.hubapi.com/oauth/v1/access-tokens/{0}";
}
