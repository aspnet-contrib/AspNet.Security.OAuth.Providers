/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.OAuth.Harvest
{
    /// <summary>
    /// Defines a set of options used by <see cref="HarvestAuthenticationHandler"/>.
    /// </summary>
    public class HarvestAuthenticationOptions : OAuthOptions
    {
        public HarvestAuthenticationOptions()
        {
            AuthenticationScheme = HarvestAuthenticationDefaults.AuthenticationScheme;
            DisplayName = HarvestAuthenticationDefaults.DisplayName;
            ClaimsIssuer = HarvestAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(HarvestAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = HarvestAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = HarvestAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = HarvestAuthenticationDefaults.UserInformationEndpoint;
        }
    }
}
