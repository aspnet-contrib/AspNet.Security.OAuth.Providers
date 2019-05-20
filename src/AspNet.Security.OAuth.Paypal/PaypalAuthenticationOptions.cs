/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace AspNet.Security.OAuth.Paypal
{
    /// <summary>
    /// Defines a set of options used by <see cref="PaypalAuthenticationHandler"/>.
    /// </summary>
    public class PaypalAuthenticationOptions : OAuthOptions
    {
        public PaypalAuthenticationOptions()
        {
            ClaimsIssuer = PaypalAuthenticationDefaults.Issuer;
            CallbackPath = PaypalAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = PaypalAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = PaypalAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = PaypalAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
            ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => user.Value<string>("user_id")?.Split('/')?.LastOrDefault());
            ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.Value<JArray>("emails")?.FirstOrDefault(email => email.Value<bool>("primary"))?.Value<string>("value"));
        }
    }
}
