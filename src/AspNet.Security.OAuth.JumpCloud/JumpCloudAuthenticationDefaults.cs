/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.JumpCloud;

/// <summary>
/// Default values used by the JumpCloud authentication provider.
/// </summary>
public static class JumpCloudAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "JumpCloud";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "JumpCloud";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "JumpCloud";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-jumpcloud";

    /// <summary>
    /// Default path format to use for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    [Obsolete("This field is obsolete and will be removed in a future version.")]
    public static readonly string AuthorizationEndpointPathFormat = "/oauth2/auth";

    /// <summary>
    /// Default path format to use for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    [Obsolete("This field is obsolete and will be removed in a future version.")]
    public static readonly string TokenEndpointPathFormat = "/oauth2/token";

    /// <summary>
    /// Default path format to use for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    [Obsolete("This field is obsolete and will be removed in a future version.")]
    public static readonly string UserInformationEndpointPathFormat = "/userinfo";

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpointPath = "/oauth2/auth";

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointPath = "/oauth2/token";

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpointPath = "/userinfo";
}
