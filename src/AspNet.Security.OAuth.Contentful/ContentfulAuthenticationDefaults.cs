/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.Contentful;

/// <summary>
/// Default values used by the Contentful authentication middleware.
/// </summary>
public static class ContentfulAuthenticationDefaults
{
    /// <summary>
    /// Default value for the <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "Contentful";

    /// <summary>
    /// Default value for the <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "Contentful";

    /// <summary>
    /// Default value for the <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "Contentful";

    /// <summary>
    /// Default value for the <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-contentful";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://be.contentful.com/oauth/authorize";

    /// <summary>
    /// Default value for the <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpointFormat = "https://be.contentful.com/oauth/token";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpoint = "https://api.contentful.com/users/me";
}
