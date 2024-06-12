/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Pipedrive;

/// <summary>
/// Defines a set of options used by <see cref="PipedriveAuthenticationHandler"/>.
/// </summary>
public class PipedriveAuthenticationOptions : OAuthOptions
{
    public PipedriveAuthenticationOptions()
    {
        ClaimsIssuer = PipedriveAuthenticationDefaults.Issuer;
        CallbackPath = PipedriveAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = PipedriveAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = PipedriveAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = PipedriveAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("base");

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.GetProperty("data").GetString("id"));
        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetProperty("data").GetString("name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetProperty("data").GetString("email"));
    }
}
