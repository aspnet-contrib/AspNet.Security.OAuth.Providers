/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Bitbucket
{
    /// <summary>
    /// Defines a set of options used by <see cref="BitbucketAuthenticationHandler"/>.
    /// </summary>
    public class BitbucketAuthenticationOptions : OAuthOptions
    {
        public BitbucketAuthenticationOptions()
        {
            AuthenticationScheme = BitbucketAuthenticationDefaults.AuthenticationScheme;
            DisplayName = BitbucketAuthenticationDefaults.DisplayName;
            ClaimsIssuer = BitbucketAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(BitbucketAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = BitbucketAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = BitbucketAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = BitbucketAuthenticationDefaults.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the address of the endpoint exposing
        /// the email addresses associated with the logged in user.
        /// </summary>
        public string UserEmailsEndpoint { get; set; } = BitbucketAuthenticationDefaults.UserEmailsEndpoint;
    }
}
