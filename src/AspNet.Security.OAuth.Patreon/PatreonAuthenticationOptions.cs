/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Patreon
{
    /// <summary>
    /// Defines a set of options used by <see cref="PatreonAuthenticationHandler"/>.
    /// </summary>
    public class PatreonAuthenticationOptions : OAuthOptions
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="PatreonAuthenticationOptions"/> class.
        /// </summary>
        public PatreonAuthenticationOptions()
        {
            AuthenticationScheme = PatreonAuthenticationDefaults.AuthenticationScheme;
            DisplayName = PatreonAuthenticationDefaults.DisplayName;
            ClaimsIssuer = PatreonAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(PatreonAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = PatreonAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = PatreonAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = PatreonAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("users");
            Scope.Add("pledges-to-me");
            Scope.Add("my-campaign");
        }
    }
}
