/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Globalization;

namespace AspNet.Security.OAuth.Okta;

/// <summary>
/// Default values used by the Okta authentication provider.
/// </summary>
public static class OktaAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "Okta";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "Okta";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "Okta";

    /// <summary>
    /// The name of the default Okta custom Authorization Server.
    /// </summary>
    public static readonly string DefaultAuthorizationServer = "default";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-okta";

    /// <summary>
    /// Default path format to use for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpointPathFormat = "/oauth2/{0}/v1/authorize";

    /// <summary>
    /// Default path format to use for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointPathFormat = "/oauth2/{0}/v1/token";

    /// <summary>
    /// Default path format to use for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpointPathFormat = "/oauth2/{0}/v1/userinfo";

#pragma warning disable CA1863
    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpointPath = string.Format(CultureInfo.InvariantCulture, AuthorizationEndpointPathFormat, DefaultAuthorizationServer);

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointPath = string.Format(CultureInfo.InvariantCulture, TokenEndpointPathFormat, DefaultAuthorizationServer);

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpointPath = string.Format(CultureInfo.InvariantCulture, UserInformationEndpointPathFormat, DefaultAuthorizationServer);
#pragma warning restore CA1863
}
