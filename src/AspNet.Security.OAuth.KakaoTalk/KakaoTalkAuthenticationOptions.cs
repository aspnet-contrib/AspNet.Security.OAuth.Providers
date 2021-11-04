/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using System.Text.Json;
using static AspNet.Security.OAuth.KakaoTalk.KakaoTalkAuthenticationConstants;

namespace AspNet.Security.OAuth.KakaoTalk
{
    /// <summary>
    /// Defines a set of options used by <see cref="KakaoTalkAuthenticationHandler"/>.
    /// </summary>
    public class KakaoTalkAuthenticationOptions : OAuthOptions
    {
        public KakaoTalkAuthenticationOptions()
        {
            ClaimsIssuer = KakaoTalkAuthenticationDefaults.Issuer;
            CallbackPath = KakaoTalkAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = KakaoTalkAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = KakaoTalkAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = KakaoTalkAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapCustomJson(ClaimTypes.Name, user =>
            {
                JsonElement property = user;
                bool hasProperty = property.TryGetProperty("kakao_account", out property)
                                && property.TryGetProperty("profile", out property)
                                && property.TryGetProperty("nickname", out property)
                                && property.ValueKind == JsonValueKind.String;
                return hasProperty
                    ? property.GetString()
                    : null;
            });
            ClaimActions.MapJsonSubKey(ClaimTypes.Email, "kakao_account", "email");
            ClaimActions.MapJsonSubKey(ClaimTypes.DateOfBirth, "kakao_account", "birthday");
            ClaimActions.MapJsonSubKey(ClaimTypes.Gender, "kakao_account", "gender");
            ClaimActions.MapJsonSubKey(ClaimTypes.MobilePhone, "kakao_account", "phone_number");
            ClaimActions.MapJsonSubKey(Claims.AgeRange, "kakao_account", "age_range");
            ClaimActions.MapJsonSubKey(Claims.YearOfBirth, "kakao_account", "birthyear");
        }
    }
}
