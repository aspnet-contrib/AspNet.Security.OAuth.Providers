/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Yandex
{
    /// <summary>
    /// Defines a set of options used by <see cref="YandexAuthenticationHandler"/>.
    /// </summary>
    public class YandexAuthenticationOptions : OAuthOptions
    {
        public YandexAuthenticationOptions()
        {
            AuthenticationScheme = YandexAuthenticationDefaults.AuthenticationScheme;
            DisplayName = YandexAuthenticationDefaults.DisplayName;
            ClaimsIssuer = YandexAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(YandexAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = YandexAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = YandexAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = YandexAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}