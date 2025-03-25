/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Linear;

/// <summary>
/// Default values used by the Linear authentication middleware.
/// </summary>
public static class LinearAuthenticationDefaults
{
    /// <summary>
    /// Default value for the <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "Linear";

    /// <summary>
    /// Default value for the <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "Linear";

    /// <summary>
    /// Default value for the <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "Linear";

    /// <summary>
    /// Default value for the <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-linear";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://linear.app/oauth/authorize";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointFormat = "https://api.linear.app/oauth/token";
}
