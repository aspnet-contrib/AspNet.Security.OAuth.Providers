/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Onshape;

/// <summary>
/// Defines a set of options used by <see cref="OnshapeAuthenticationHandler"/>.
/// </summary>
public class OnshapeAuthenticationOptions : OAuthOptions
{
    public OnshapeAuthenticationOptions()
    {
        ClaimsIssuer = OnshapeAuthenticationDefaults.Issuer;

        CallbackPath = OnshapeAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = OnshapeAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = OnshapeAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = OnshapeAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
    }
}
