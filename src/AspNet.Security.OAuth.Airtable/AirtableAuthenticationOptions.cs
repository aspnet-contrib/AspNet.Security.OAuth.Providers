/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Airtable;

/// <summary>
/// Defines a set of options used by <see cref="AirtableAuthenticationHandler"/>.
/// </summary>
public class AirtableAuthenticationOptions : OAuthOptions
{
    public AirtableAuthenticationOptions()
    {
        ClaimsIssuer = AirtableAuthenticationDefaults.Issuer;
        CallbackPath = AirtableAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AirtableAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = AirtableAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = AirtableAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("user.email:read");

        ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.GetString("id"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("email"));
    }
}
