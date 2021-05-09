// Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
// See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
// for more information concerning the license and the contributors participating to this project.

using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace AspNet.Security.OAuth.Keycloak
{
    /// <summary>
    /// A class used to setup defaults for all <see cref="KeycloakAuthenticationOptions"/>.
    /// </summary>
    public class KeycloakPostConfigureOptions : IPostConfigureOptions<KeycloakAuthenticationOptions>
    {
        public void PostConfigure([NotNull] string name, [NotNull] KeycloakAuthenticationOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                options.AuthorizationEndpoint = options.BaseUrl + KeycloakAuthenticationDefaults.AuthorizationEndpoint;
                options.TokenEndpoint = options.BaseUrl + KeycloakAuthenticationDefaults.TokenEndpoint;
                options.UserInformationEndpoint = options.BaseUrl + KeycloakAuthenticationDefaults.UserInformationEndpoint;
            }
        }
    }
}
