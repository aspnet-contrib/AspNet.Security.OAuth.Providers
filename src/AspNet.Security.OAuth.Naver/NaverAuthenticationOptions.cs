/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Naver.NaverAuthenticationConstants;

namespace AspNet.Security.OAuth.Naver;

/// <summary>
/// Defines a set of options used by <see cref="NaverAuthenticationHandler"/>.
/// </summary>
public class NaverAuthenticationOptions : OAuthOptions
{
    public NaverAuthenticationOptions()
    {
        ClaimsIssuer = NaverAuthenticationDefaults.Issuer;
        CallbackPath = NaverAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = NaverAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = NaverAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = NaverAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(Claims.Nickname, "nickname");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
        ClaimActions.MapJsonKey(Claims.Age, "age");
        ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
        ClaimActions.MapJsonKey(Claims.ProfileImage, "profile_image");
        ClaimActions.MapJsonKey(Claims.YearOfBirth, "birthyear");
        ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "mobile");
    }
}
