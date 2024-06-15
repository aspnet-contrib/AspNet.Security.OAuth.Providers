/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Docusign;

/// <summary>
/// Defines a set of options used by <see cref="DocusignAuthenticationHandler"/>.
/// </summary>
public class DocusignAuthenticationOptions : OAuthOptions
{
    public DocusignAuthenticationOptions()
    {
        ClaimsIssuer = DocusignAuthenticationDefaults.Issuer;
        CallbackPath = DocusignAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = DocusignAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = DocusignAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = DocusignAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapCustomJson(ClaimTypes.Name, user => user.GetString("name"));
        ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("email"));
        ClaimActions.MapCustomJson(ClaimTypes.GivenName, user => user.GetString("given_name"));
        ClaimActions.MapCustomJson(ClaimTypes.Surname, user => user.GetString("family_name"));
    }
}
