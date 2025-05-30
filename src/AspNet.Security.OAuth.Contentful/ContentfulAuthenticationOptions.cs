/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Contentful;

/// <summary>
/// Defines a set of options used by <see cref="ContentfulAuthenticationHandler"/>.
/// </summary>
public class ContentfulAuthenticationOptions : OAuthOptions
{
    public ContentfulAuthenticationOptions()
    {
        ClaimsIssuer = ContentfulAuthenticationDefaults.Issuer;
        CallbackPath = ContentfulAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = ContentfulAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = ContentfulAuthenticationDefaults.TokenEndpointFormat;
        UserInformationEndpoint = ContentfulAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "sys", "id");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
    }
}
