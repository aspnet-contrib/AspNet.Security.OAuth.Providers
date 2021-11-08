/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.ExactOnline.ExactOnlineAuthenticationConstants;

namespace AspNet.Security.OAuth.ExactOnline;

/// <summary>
/// Defines a set of options used by <see cref="ExactOnlineAuthenticationHandler"/>.
/// </summary>
public class ExactOnlineAuthenticationOptions : OAuthOptions
{
    public ExactOnlineAuthenticationOptions()
    {
        ClaimsIssuer = ExactOnlineAuthenticationDefaults.Issuer;

        CallbackPath = ExactOnlineAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = ExactOnlineAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = ExactOnlineAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = ExactOnlineAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "UserID");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "FirstName");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "LastName");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "FullName");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "Email");
        ClaimActions.MapJsonKey(Claims.Division, "CurrentDivision");
        ClaimActions.MapJsonKey(Claims.Company, "DivisionCustomerName");
    }
}
