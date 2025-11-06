/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Etsy;

/// <summary>
/// Default values used by the Etsy authentication middleware.
/// </summary>
public static class EtsyAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "Etsy";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "Etsy";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "Etsy";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    /// <value>
    /// "/signin-etsy"
    /// </value>
    public static readonly string CallbackPath = "/signin-etsy";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://www.etsy.com/oauth/connect";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpoint = "https://api.etsy.com/v3/public/oauth/token";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/> <see href="https://developers.etsy.com/documentation/reference/#operation/getMe">Etsy getMe Endpoint</see>.
    /// </summary>
    public static readonly string UserInformationEndpoint = "https://openapi.etsy.com/v3/application/users/me";

    /// <summary>
    /// Default value for the Etsy v3 API Base URI
    /// </summary>
    /// <value>
    /// https://openapi.etsy.com/v3/application/
    /// </value>
    public static readonly string EtsyBaseUri = "https://openapi.etsy.com/v3/application/";

    /// <summary>
    /// Default value for Etsy user details endpoint path <see href="https://developers.etsy.com/documentation/reference/#operation/getUser">getUser</see>.
    /// </summary>
    /// <value>
    /// "/users/{user_id}"
    /// </value>
    public static readonly string UserDetailsPath = "/users/{user_id}";
}
