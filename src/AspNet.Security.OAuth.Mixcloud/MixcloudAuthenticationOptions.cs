/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using static AspNet.Security.OAuth.Mixcloud.MixcloudAuthenticationConstants;

namespace AspNet.Security.OAuth.Mixcloud;

/// <summary>
/// Defines a set of options used by <see cref="MixcloudAuthenticationHandler"/>.
/// </summary>
public class MixcloudAuthenticationOptions : OAuthOptions
{
    public MixcloudAuthenticationOptions()
    {
        ClaimsIssuer = MixcloudAuthenticationDefaults.Issuer;
        CallbackPath = MixcloudAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = MixcloudAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = MixcloudAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = MixcloudAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "key");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(Claims.FullName, "name");
        ClaimActions.MapJsonKey(Claims.Biography, "biog");
        ClaimActions.MapJsonKey(Claims.City, "city");
        ClaimActions.MapJsonKey(ClaimTypes.Country, "country");

        ClaimActions.MapJsonKey(Claims.ProfileUrl, "url");
        ClaimActions.MapJsonSubKey(Claims.ProfileImageUrl, "pictures", "320wx320h");
        ClaimActions.MapJsonSubKey(Claims.ProfileThumbnailUrl, "pictures", "thumbnail");
    }
}
