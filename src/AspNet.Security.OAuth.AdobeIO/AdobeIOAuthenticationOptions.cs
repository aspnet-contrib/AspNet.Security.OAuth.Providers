/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.AdobeIO.AdobeIOAuthenticationConstants;

namespace AspNet.Security.OAuth.AdobeIO;

/// <summary>
/// Defines a set of options used by <see cref="AdobeIOAuthenticationHandler"/>.
/// </summary>
public class AdobeIOAuthenticationOptions : OAuthOptions
{
    public AdobeIOAuthenticationOptions()
    {
        ClaimsIssuer = AdobeIOAuthenticationDefaults.Issuer;
        CallbackPath = AdobeIOAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = AdobeIOAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = AdobeIOAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = AdobeIOAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("openid");
        Scope.Add("AdobeID");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
        ClaimActions.MapJsonSubKey(ClaimTypes.Country, "address", "country");
        ClaimActions.MapJsonKey(Claims.AccountType, "account_type");
        ClaimActions.MapJsonKey(Claims.EmailVerified, "email_verified");
    }
}
