﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Dropbox;

/// <summary>
/// Defines a set of options used by <see cref="DropboxAuthenticationHandler"/>.
/// </summary>
public class DropboxAuthenticationOptions : OAuthOptions
{
    /// <summary>
    /// Gets or sets what the response type from Dropbox should be: online, offline or legacy.
    /// </summary>
    public string? AccessType { get; set; }

    public DropboxAuthenticationOptions()
    {
        ClaimsIssuer = DropboxAuthenticationDefaults.Issuer;

        CallbackPath = DropboxAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = DropboxAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = DropboxAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = DropboxAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "account_id");
        ClaimActions.MapJsonSubKey(ClaimTypes.Name, "name", "display_name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    }
}
