/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.MailRu.MailRuAuthenticationConstants;

namespace AspNet.Security.OAuth.MailRu
{
    /// <summary>
    /// Defines a set of options used by <see cref="MailRuAuthenticationHandler"/>.
    /// </summary>
    public class MailRuAuthenticationOptions : OAuthOptions
    {
        public MailRuAuthenticationOptions()
        {
            ClaimsIssuer = MailRuAuthenticationDefaults.Issuer;
            CallbackPath = MailRuAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = MailRuAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MailRuAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MailRuAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(Claims.ImageUrl, "image");
        }
    }
}
