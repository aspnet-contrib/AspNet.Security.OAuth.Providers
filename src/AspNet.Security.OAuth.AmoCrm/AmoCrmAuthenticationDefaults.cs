/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.AmoCrm;

/// <summary>
/// Default values used by the amoCRM authentication middleware.
/// </summary>
public static class AmoCrmAuthenticationDefaults
{
    /// <summary>
    /// Default value for the <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "amoCRM";

    /// <summary>
    /// Default value for the <see cref="Microsoft.AspNetCore.Authentication.AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "amoCRM";

    /// <summary>
    /// Default value for the <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "amoCRM";

    /// <summary>
    /// Default value for the <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-amocrm";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://www.amocrm.ru/oauth";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointFormat = "https://{0}.amocrm.ru/oauth2/access_token";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpointFormat = "https://{0}.amocrm.ru/v3/user";
}
