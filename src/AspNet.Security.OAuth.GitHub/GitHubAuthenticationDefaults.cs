﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.GitHub;

/// <summary>
/// Default values used by the GitHub authentication middleware.
/// </summary>
public static class GitHubAuthenticationDefaults
{
    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.Name"/>.
    /// </summary>
    public const string AuthenticationScheme = "GitHub";

    /// <summary>
    /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
    /// </summary>
    public static readonly string DisplayName = "GitHub";

    /// <summary>
    /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
    /// </summary>
    public static readonly string Issuer = "GitHub";

    /// <summary>
    /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
    /// </summary>
    public static readonly string CallbackPath = "/signin-github";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public static readonly string TokenEndpoint = "https://github.com/login/oauth/access_token";

    /// <summary>
    /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpoint = "https://api.github.com/user";

    /// <summary>
    /// Default value for <see cref="GitHubAuthenticationOptions.UserEmailsEndpoint"/>.
    /// </summary>
    public static readonly string UserEmailsEndpoint = "https://api.github.com/user/emails";

    /// <summary>
    /// Default path to use for the GitHub Enterprise v3 REST API.
    /// </summary>
    public static readonly string EnterpriseApiPath = "/api/v3";

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
    /// </summary>
    public static readonly string AuthorizationEndpointPath = "/login/oauth/authorize";

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.TokenEndpoint"/>.
    /// </summary>
    public const string TokenEndpointPath = "/login/oauth/access_token";

    /// <summary>
    /// Default path to use for <see cref="OAuthOptions.UserInformationEndpoint"/>.
    /// </summary>
    public static readonly string UserInformationEndpointPath = "/user";

    /// <summary>
    /// Default path to use for <see cref="GitHubAuthenticationOptions.UserEmailsEndpoint"/>.
    /// </summary>
    public static readonly string UserEmailsEndpointPath = "/user/emails";
}
