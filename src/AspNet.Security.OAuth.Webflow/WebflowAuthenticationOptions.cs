/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Webflow;

/// <summary>
/// Defines a set of options used by <see cref="WebflowAuthenticationHandler"/>.
/// </summary>
public class WebflowAuthenticationOptions : OAuthOptions
{
    public WebflowAuthenticationOptions()
    {
        ClaimsIssuer = WebflowAuthenticationDefaults.Issuer;

        CallbackPath = WebflowAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = WebflowAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = WebflowAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = WebflowAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("authorized_user:read");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
    }
}
