/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.Bitbucket.BitbucketAuthenticationConstants;

namespace AspNet.Security.OAuth.Bitbucket
{
    /// <summary>
    /// Defines a set of options used by <see cref="BitbucketAuthenticationHandler"/>.
    /// </summary>
    public class BitbucketAuthenticationOptions : OAuthOptions
    {
        public BitbucketAuthenticationOptions()
        {
            ClaimsIssuer = BitbucketAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(BitbucketAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BitbucketAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BitbucketAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BitbucketAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "account_id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(Claims.DisplayName, "display_name");
            ClaimActions.MapJsonKey(Claims.Website, "website");
        }

        /// <summary>
        /// Gets or sets the address of the endpoint exposing
        /// the email addresses associated with the logged in user.
        /// </summary>
        public string UserEmailsEndpoint { get; set; } = BitbucketAuthenticationDefaults.UserEmailsEndpoint;
    }
}
