/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Yahoo
{
    /// <summary>
    /// Defines a set of options used by <see cref="YahooAuthenticationHandler"/>.
    /// </summary>
    public class YahooAuthenticationOptions : OAuthOptions
    {
        public YahooAuthenticationOptions()
        {
            ClaimsIssuer = YahooAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(YahooAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = YahooAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = YahooAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = YahooAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "guid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey("urn:yahoo:familyname", "familyName");
            ClaimActions.MapJsonKey("urn:yahoo:givenname", "givenName");
            ClaimActions.MapJsonKey("urn:yahoo:profile", "profileUrl");
            ClaimActions.MapJsonKey("urn:yahoo:profileimage", "imageUrl");
        }
    }
}