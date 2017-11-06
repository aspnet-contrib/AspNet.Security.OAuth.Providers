/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

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

            CallbackPath = new PathString(MailRuAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = MailRuAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = MailRuAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = MailRuAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey("urn:MailRu:nick", "nick");
            ClaimActions.MapJsonKey("urn:MailRu:pic:link", "pic");
            ClaimActions.MapJsonKey("urn:MailRu:pic_small:link", "pic_small");
        }
    }
}
