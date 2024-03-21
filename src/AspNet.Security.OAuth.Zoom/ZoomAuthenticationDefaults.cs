/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Zoom;

/// <summary>
/// Default values used by the Zoom authentication middleware.
/// </summary>
public class ZoomAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "Zoom";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "Zoom";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "Zoom";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-zoom";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://zoom.us/oauth/authorize";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpoint = "https://zoom.us/oauth/token";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpoint = "https://api.zoom.us/v2/users/me";
}
