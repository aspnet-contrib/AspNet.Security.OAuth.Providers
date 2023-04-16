/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;

namespace AspNet.Security.OAuth.Kook;

/// <summary>
/// Defines a set of options used by <see cref="KookAuthenticationHandler"/>.
/// </summary>
public class KookAuthenticationOptions : OAuthOptions
{
    public KookAuthenticationOptions()
    {
        ClaimsIssuer = KookAuthenticationDefaults.Issuer;
        CallbackPath = KookAuthenticationDefaults.CallbackPath;
        AuthorizationEndpoint = KookAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = KookAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = KookAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "mobile");
        ClaimActions.MapJsonKey(KookAuthenticationConstants.Claims.IdentifyNumber, "identify_num");
        ClaimActions.MapJsonKey(KookAuthenticationConstants.Claims.OperatingSystem, "os");
        ClaimActions.MapJsonKey(KookAuthenticationConstants.Claims.AvatarUrl, "avatar");
        ClaimActions.MapJsonKey(KookAuthenticationConstants.Claims.BannerUrl, "banner");
        ClaimActions.MapJsonKey(KookAuthenticationConstants.Claims.IsMobileVerified, "mobile_verified");

        Scope.Add("get_user_info");
    }
}
