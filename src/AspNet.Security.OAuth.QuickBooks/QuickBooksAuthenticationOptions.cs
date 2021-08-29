/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.QuickBooks.QuickBooksAuthenticationConstants;

namespace AspNet.Security.OAuth.QuickBooks
{
    /// <summary>
    /// Defines a set of options used by <see cref="QuickBooksAuthenticationHandler"/>.
    /// </summary>
    public class QuickBooksAuthenticationOptions : OAuthOptions
    {
        private readonly string scopeValOpenId = "openid email profile phone address";

        public QuickBooksAuthenticationOptions()
        {
            ClaimsIssuer = QuickBooksAuthenticationDefaults.Issuer;
            CallbackPath = QuickBooksAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = QuickBooksAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = QuickBooksAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = QuickBooksAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add(scopeValOpenId);

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
            ClaimActions.MapJsonKey(ClaimTypes.MobilePhone, "mobilephone");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(Claims.EmailVerified, "emailVerified");
        }
    }
}
