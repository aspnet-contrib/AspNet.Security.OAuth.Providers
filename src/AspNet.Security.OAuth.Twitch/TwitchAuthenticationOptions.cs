/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Twitch
{
    /// <summary>
    /// Defines a set of options used by <see cref="TwitchAuthenticationHandler"/>.
    /// </summary>
    public class TwitchAuthenticationOptions : OAuthOptions
    {
        public TwitchAuthenticationOptions()
        {
            AuthenticationScheme = TwitchAuthenticationDefaults.AuthenticationScheme;
            DisplayName = TwitchAuthenticationDefaults.DisplayName;
            ClaimsIssuer = TwitchAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(TwitchAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = TwitchAuthenticationDefaults.AuthorizationEndPoint;
            TokenEndpoint = TwitchAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = TwitchAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("user:read:email");
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether the "force_verify=true" flag should be sent to Twitch.
        /// When set to <c>true</c>, Twitch displays the consent screen for every authorization request.
        /// When left to <c>false</c>, the consent screen is skipped if the user is already logged in.
        /// </summary>
        public bool ForceVerify { get; set; }
    }
}
